using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Interop;
using System.Windows.Forms;

namespace PRYANIK_Plugin.Services
{
    public static class AddComment
    {
        internal static Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        internal static string textComment;

        internal static void GettingDataForAComment()
        {
            Selection storedSelection = doc.CurrentSelection;

            foreach (ModelItem selectedItem in storedSelection.GetSelectedItems())
            {
                textComment += $"{elementPath(selectedItem)}-ID: {elementID(selectedItem)};\n";
            }

            string elementPath(ModelItem item)
            {
                string elementFile = item?.Parent?.Parent?.Parent?.Parent?.PropertyCategories.FindPropertyByDisplayName("Элемент", "Файл источника")?.Value.ToString();
                string elementType = item?.Parent?.Parent?.Parent?.Parent?.Parent?.PropertyCategories.FindPropertyByDisplayName("Элемент", "Тип")?.Value.ToString();

                if (elementType == "DisplayString:Экземпляр" || elementType == "DisplayString:Ссылка")
                {
                    return textFilter(item.Parent.Parent.Parent.Parent.Parent.PropertyCategories.FindPropertyByDisplayName("Элемент", "Имя").Value.ToString());
                }
                else if (elementFile == null && elementType == null)
                {
                    MessageBox.Show("Ошибка при получении имени файла! Проверьте созданный вид.\nПри создании комментария был выбран элемент: {item.DisplayName}");
                    return "Ошибка-ИМЯ";
                }
                else
                {
                    return textFilter(elementFile);
                }
            };

            string elementID(ModelItem item)
            {
                string elementIDValues = item?.PropertyCategories.FindPropertyByDisplayName("ID объекта", "Значение")?.Value.ToString();

                if (elementIDValues == null)
                {
                    MessageBox.Show("Ошибка при получении ID элемента! Проверьте созданный вид.\nПри создании комментария был выбран элемент: {item.DisplayName}");
                    return "Ошибка-ID";
                }
                else
                {
                    return textFilter(elementIDValues);
                }
            };

            string textFilter(string text)
            {
                int endIndex = text.IndexOf(".rvt");
                string cutText = endIndex != -1 ? text.Substring(0, endIndex) : text;
                return cutText.Replace("DisplayString:", "");
            }
        }

        internal static Point2D ScreenToCameraSpace(Point2D pointScreenSpace)
        {
            LcOaViewer viewer = doc.ActiveView.Viewer;
            var pointWindowSpace = LcOpRedline.ScreenToWindowSpace(viewer, pointScreenSpace);
            var pointCameraSpace = LcOpRedline.WindowToCameraSpace(viewer, pointWindowSpace);

            return pointCameraSpace;
        }

        internal static void CreateViewpoint()
        {
            Viewpoint curentVievpoint = doc.CurrentViewpoint.Value;
            SavedViewpoint newViewpoint = new SavedViewpoint(curentVievpoint);
            newViewpoint.DisplayName = "_Вид";
            doc.SavedViewpoints.AddCopy(newViewpoint);

            var viewpoints = doc.SavedViewpoints;
            int newIndex = viewpoints.Value.Count - 1;
            SavedViewpoint viewpointCopy = viewpoints.Value[newIndex].CreateCopy() as SavedViewpoint;

            Point2D point2D = new Point2D(155, 830);
            var redlineText = new LcOpRedlineText(textComment, ScreenToCameraSpace(point2D));
            redlineText.SetLineThickness(5);
            redlineText.SetLineColor(Color.White);

            viewpointCopy.Redlines.Add(redlineText);
            viewpoints.ReplaceWithCopy(newIndex, viewpointCopy);

            SavedItem item = doc.SavedViewpoints.Value[newIndex];
            SavedViewpoint savedViewpoint = item as SavedViewpoint;
            Viewpoint _vievpoint = savedViewpoint.Viewpoint;
            doc.SavedViewpoints.CurrentSavedViewpoint = item;

            doc.CurrentSelection.Clear();
            textComment = "";
        }
    }
}