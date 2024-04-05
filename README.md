# LcOpRedlineTextPlugin

<p align="center">
  <img src="https://github.com/xPRYANIKx/LcOpRedlineTextPlugin/assets/92644479/ac08f543-8996-4f63-bebd-cd507b72543e">
</p>  
<p>Плагин для <b>Autodesk Navisworks</b>, который создаёт новую точку обзора с <b>LcOpRedlineText</b> по заданным критериям.</p>  

---
## Принцип работы
1. В открытой модели пользователю необходимо выделить два элемента
2. После этого пользователю необходимо нажать кнопку **"Добавить комментарий"** на панели **"PRYANIK_Plugin"** или нажать на клавиатуре клавишу **"Q"**

**Примечание:** другую горячую клавишу можно задать в файле **"Main.cs"** *(Q - key == 81)*. Подробнее читать [здесь](https://learn.microsoft.com/ru-ru/office/vba/language/reference/user-interface-help/keycode-constants).

```c#
        public override bool KeyUp(View view, KeyModifiers modifier, ushort key, double timeOffset)
        {
            if (Application.ActiveDocument.CurrentSelection.SelectedItems.Count == 2 && key == 81)
            {
                AddComment.GettingDataForAComment();
                AddComment.CreateViewpoint();
                return true;
            }

            return base.KeyUp(view, modifier, key, timeOffset);
        }
```

3. После этого на панели **"Сохранённые точки обзора"** появится новая точка обзора с именем **"_Вид"**, на которой будет присутствовать **LcOpRedlineText** по заданным критериям *(имена элементов)*.
<p align="center">
  <img src="https://github.com/xPRYANIKx/LcOpRedlineTextPlugin/assets/92644479/2b039325-f03d-424a-903c-3efbb11e3431">
</p>  

**Примечание:** логика формирования **LcOpRedlineText** задаётся в **"Services/AddComment.cs"**:
* В зависимости от установленого языка в **Autodesk Navisworks** названия полей для **"FindPropertyByDisplayName"** будут различаться  
```c#
         string elementFile = item?.Parent?.Parent?.Parent?.Parent?.PropertyCategories.FindPropertyByDisplayName("Элемент", "Файл источника")?.Value.ToString();
         string elementType = item?.Parent?.Parent?.Parent?.Parent?.Parent?.PropertyCategories.FindPropertyByDisplayName("Элемент", "Тип")?.Value.ToString();
```
* Метод **"textFilter"** представляет собой дополнительный фильтр для формирования имени элемента
```c#
            string textFilter(string text)
            {
                int endIndex = text.IndexOf(".rvt");
                string cutText = endIndex != -1 ? text.Substring(0, endIndex) : text;
                return cutText.Replace("DisplayString:", "");
            }
```
* Имя вида можно задать в методе **"CreateViewpoint** при помощи **"newViewpoint.DisplayName"**
```c#
        internal static void CreateViewpoint()
        {
            Viewpoint curentVievpoint = doc.CurrentViewpoint.Value;
            SavedViewpoint newViewpoint = new SavedViewpoint(curentVievpoint);
            newViewpoint.DisplayName = "_Вид";
            doc.SavedViewpoints.AddCopy(newViewpoint);
            ...
```
