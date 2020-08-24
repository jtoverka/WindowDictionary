using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;

namespace AutoCAD_Windows.Resources
{
    public static class UILibrary
    {
        public static bool IsTextAllowed(string text, System.TypeCode type)
        {
            return type switch
            {
                TypeCode.Empty => text == "",
                TypeCode.Object => false,
                TypeCode.DBNull => false,
                TypeCode.Boolean => !(new Regex("[^0-9.-]+").IsMatch(text)),
                TypeCode.Char => text.Length == 1,
                TypeCode.SByte => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.Byte => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.Int16 => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.UInt16 => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.Int32 => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.UInt32 => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.Int64 => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.UInt64 => !(new Regex("[^0-9-]+").IsMatch(text)),
                TypeCode.Single => !(new Regex("[^0-9.-]+").IsMatch(text)),
                TypeCode.Double => !(new Regex("[^0-9.-]+").IsMatch(text)),
                TypeCode.Decimal => !(new Regex("[^0-9.-]+").IsMatch(text)),
                TypeCode.DateTime => false,
                TypeCode.String => (new Regex("[ A-Za-z0-9_-]").IsMatch(text)),
                _ => false,
            };
        }

        public static int CountInString(string text, char match)
        {
            return text.Split(match).Length-1;
        }
        /*
        private void TextBox_Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewTextInput(sender, e);
        }
        */
        public static void TextBox_Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var box = sender as TextBox;

            char[] boxText = box.Text.ToArray();
            char[] addText = e.Text.ToArray();

            int caret = box.CaretIndex;

            char[] newText = new char[boxText.Length + addText.Length];

            for (int i = 0; i < newText.Length; i++)
            {
                if (i < caret)
                    newText[i] = boxText[i];
                else if (i >= (addText.Length + caret))
                    newText[i] = boxText[i - addText.Length];
                else
                    newText[i] = addText[i - caret];
            }

            string text = new string(newText);

            if (text == "")
                return;

            if (IsTextAllowed(text, TypeCode.Double)
                && (CountInString(text, ' ') == 0)
                && (CountInString(text, '.') <= 1)
                && (
                    ((CountInString(text, '-') == 0))
                    || ((CountInString(text, '-') == 1) && text[0] == '-')
                   )
               )
                e.Handled = false;
            else
                e.Handled = true;
        }
        /*
        private void TextBox_Integer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Integer_PreviewKeyDown(sender, e);
        }
        */
        public static void TextBox_Double_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e?.Key == Key.Space)
                e.Handled = true;
        }
        /*
        private void TextBox_Integer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_Integer_PreviewTextInput(sender, e);
        }
        */
        public static void TextBox_Integer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender?.GetType() == typeof(TextBox))
            {
                var box = sender as TextBox;
                
                char[] boxText = box.Text.ToArray();
                char[] addText = e.Text.ToArray();

                int caret = box.CaretIndex;

                char[] newText = new char[boxText.Length + addText.Length];

                for (int i = 0; i < newText.Length; i++)
                {
                    if (i < caret)
                        newText[i] = boxText[i];
                    else if (i >= (addText.Length + caret))
                        newText[i] = boxText[i - addText.Length];
                    else
                        newText[i] = addText[i - caret];
                }

                string text = new string(newText);

                if (text == "")
                    return;

                if (IsTextAllowed(text, TypeCode.Int32) 
                    && (CountInString(text, ' ') == 0)
                    && (CountInString(text, '.') == 0)
                    && (
                        ((CountInString(text, '-') == 0))
                        || ((CountInString(text, '-') == 1) && text[0] == '-')
                       )
                   )
                    e.Handled = false;
                else
                    e.Handled = true;
            }
        }
        /*
        private void TextBox_Double_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewKeyDown(sender, e);
        }
        */
        public static void TextBox_Integer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e?.Key == Key.Space)
                e.Handled = true;
        }
        /*
        private void TextBox_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            UILibrary.TextBox_PreviewCanExecute(sender, e);
        }
        */
        public static void TextBox_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((e.Command == ApplicationCommands.Cut)
                || (e.Command == ApplicationCommands.Copy)
                || (e.Command == ApplicationCommands.Paste)
                || (e.Command == ApplicationCommands.Delete)
                || (e.Command == ApplicationCommands.Undo)
                || (e.Command == ApplicationCommands.Redo))
            {
                e.Handled = true;
                e.CanExecute = false;
            }
        }
        /*
         private void TextBox_String_PreviewTextInput(object sender, TextCompositionEventArgs e)
         {
            UILibrary.TextBox_String_PreviewTextInput(sender, e);
         }
         */
        public static void TextBox_String_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender?.GetType() == typeof(TextBox))
            {
                if ((e.Text != null) && (e.Text != ""))
                {
                    if (
                        !char.IsWhiteSpace(e.Text[0])
                        && !(IsTextAllowed(e.Text, TypeCode.String))
                        && !(e.Text.Split('.').Length - 1 < 2)
                       )
                        e.Handled = true;
                }
            }
        }
        public static void ListBox_String_Remove(object sender, RoutedEventArgs e, ref string StringCollection)
        {
            // suppress unused parameter message
            _ = e;

            if (sender?.GetType() == typeof(Button))
            {
                var button = sender as Button;
                var list = button.Tag as ListBox;
                int index = list.SelectedIndex;
                
                string[] layers = StringCollection.Split(',');
                var count = layers.Count();

                string fanInOutLayers = "";
                int c = 0;

                if (index != 0)
                {
                    if (count > 0)
                        fanInOutLayers = layers[0];
                }
                else if (count > 1)
                {
                    c++;
                    fanInOutLayers = layers[1];
                }

                for (int i = 1 + c; i < count; i++)
                {
                    string item = layers[i];

                    if (index != i)
                        fanInOutLayers += "," + item;
                }

                if (fanInOutLayers == "")
                    fanInOutLayers = null;

                StringCollection = fanInOutLayers;
            }
        }
        public static void Up_Button_Click(object sender, RoutedEventArgs e, IList selection, ObservableCollection<object> collection)
        {
            _ = e;
            _ = sender;
            int[] oldIndices = new int[selection.Count];
            int[] newIndices = new int[selection.Count];

            for (int i = 0; i < selection.Count; i++)
                oldIndices[i] = collection.IndexOf(selection[i]);

            Array.Sort(oldIndices);

            for (int i = 0; i < selection.Count; i++)
            {
                if (oldIndices[i] > i)
                    newIndices[i] = oldIndices[i] - 1;
                else
                    newIndices[i] = oldIndices[i];
            }
            Array.Sort(newIndices, oldIndices);

            for (int i = 0; i < selection.Count; i++)
            {
                if (oldIndices[i] != newIndices[i])
                    collection.Move(oldIndices[i], newIndices[i]);
            }
        }
        public static void Down_Button_Click(object sender, RoutedEventArgs e, IList selection, ObservableCollection<object> collection)
        {
            _ = e;
            _ = sender;
            int lastBoxIndex = collection.Count - 1;

            int[] oldIndices = new int[selection.Count];
            int[] newIndices = new int[selection.Count];

            for (int i = 0; i < selection.Count; i++)
                oldIndices[i] = collection.IndexOf(selection[i]);

            Array.Sort(oldIndices);

            for (int i = 0, j = selection.Count - 1; i < selection.Count; i++, j--)
            {
                if (oldIndices[i] < lastBoxIndex - j)
                    newIndices[i] = oldIndices[i] + 1;
                else
                    newIndices[i] = oldIndices[i];
            }

            Array.Sort(newIndices, oldIndices);

            for (int i = selection.Count - 1; i >= 0; i--)
            {
                if (oldIndices[i] != newIndices[i])
                    collection.Move(oldIndices[i], newIndices[i]);
            }
        }
        public static void Top_Button_Click(object sender, RoutedEventArgs e, IList selection, ObservableCollection<object> collection)
        {
            _ = e;
            _ = sender;
            int[] oldIndices = new int[selection.Count];
            int[] newIndices = new int[selection.Count];

            for (int i = 0; i < selection.Count; i++)
            {
                oldIndices[i] = collection.IndexOf(selection[i]);
                newIndices[i] = i;
            }
            Array.Sort(oldIndices);
            Array.Sort(newIndices, oldIndices);

            for (int i = 0; i < selection.Count; i++)
            {
                if (oldIndices[i] != newIndices[i])
                    collection.Move(oldIndices[i], newIndices[i]);
            }
        }
        public static void Bottom_Button_Click(object sender, RoutedEventArgs e, IList selection, ObservableCollection<object> collection)
        {
            _ = e;
            _ = sender;
            int lastBoxIndex = collection.Count - 1;

            int[] oldIndices = new int[selection.Count];
            int[] newIndices = new int[selection.Count];

            for (int i = 0, j = lastBoxIndex;
                i < selection.Count;
                i++, j--)
            {
                oldIndices[i] = collection.IndexOf(selection[i]);
                newIndices[i] = j;
            }
            Array.Sort(oldIndices);
            Array.Sort(newIndices);

            for (int i = selection.Count - 1; i >= 0; i--)
            {
                if (oldIndices[i] != newIndices[i])
                    collection.Move(oldIndices[i], newIndices[i]);
            }
        }
        public static void Remove_Button_Click(object sender, RoutedEventArgs e, IList selection, ObservableCollection<object> collection)
        {
            _ = e;
            _ = sender;
            for (int i = selection.Count - 1; i >= 0; i--)
            {
                collection.Remove(selection[i]);
            }
        }
    }
}
