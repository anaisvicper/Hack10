using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    public class ListCreated {
        public static List<ListView> lists = new List<ListView>();
        public static List<ListView> tasks_lists = new List<ListView>();
    }
    public class ColorPickerH
    {
        public static SolidColorBrush sd;
        public static bool first= true;
        private static int last_color = 0;
        public static List<int> colors = new List<int>();
        public static Color getColorDifferent(int c){
            return getColor((last_color + 1)%4);
        }
        public static Color getColor(int c) {
            last_color = c;
            switch (c)
            {
                case 0:
                    return Colors.Blue;
                case 1:
                    return Colors.Green;
                case 2:
                    return Colors.Red;
            }
            return Colors.Purple;
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void add_task(ListView l)
        {
            CheckBox cb = new CheckBox();
           // StackPanel sp = new StackPanel();
           // sp.Orientation = Orientation.Horizontal;
            TextBox tb = new TextBox();
            tb.Text = "Indica aqui la tarea";
           // sp.Children.Add(tb);
            cb.Content = tb;
            l.Items.Add(cb);
        }

        private void add_task_clicked(object sender, RoutedEventArgs e){
            int n_lista = int.Parse(((Button) sender).Name);
            add_task(ListCreated.tasks_lists.ElementAt(n_lista));
        }


        private void changeColor(object sender, RoutedEventArgs e){
            ((ListView)((ColorPicker.ColorPicker)sender).Parent).Background = ((ColorPicker.ColorPicker)sender).SelectedColor;
        }


        private void elim_lista(object sender, RoutedEventArgs e){
            ((ListView)((StackPanel)((Button)sender).Parent).Parent).Items.Clear();
        }

        private void add_clicked(object sender, RoutedEventArgs e){
            ListView mainL = new ListView();
            //Titulo
            TextBox tbTitle = new TextBox();
            tbTitle.Text = "Título de la lista";
            tbTitle.HorizontalAlignment = HorizontalAlignment.Center;
            mainL.Items.Add(tbTitle);
            // StackPanel - Fecha
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            TextBlock t = new TextBlock();
            t.Text = "Fecha límite de la lista: ";
            t.Foreground = new SolidColorBrush(Colors.Aquamarine);
            sp.Children.Add(t);

            DatePicker dp = new DatePicker();
            //dp.DateChanged += fecha_chng;
            sp.Children.Add(dp);

            TimePicker tp = new TimePicker();
            //tp.TimeChanged += time_chng;
            sp.Children.Add(tp);

            Button btn_submit = new Button();
            btn_submit.Content = "Guardar";
            btn_submit.Click += createNotificationTask;
            btn_submit.Background = new SolidColorBrush(Colors.Aquamarine);
            sp.Children.Add(btn_submit);

            mainL.Items.Add(sp);
            //Lista de tareas
            ListView l = new ListView();
            mainL.Items.Add(l);
            ListCreated.tasks_lists.Add(l);

            //StackPanel - menu
            StackPanel menu = new StackPanel();
            
            Button btnAddTask = new Button();
            btnAddTask.Content = "Añadir tarea";
            btnAddTask.Background = new SolidColorBrush(Colors.Aquamarine);
            btnAddTask.HorizontalAlignment = HorizontalAlignment.Left;
            btnAddTask.VerticalAlignment = VerticalAlignment.Bottom;
            btnAddTask.Name = stackP.Children.Count+"";
            btnAddTask.Click += add_task_clicked;
            menu.Children.Add(btnAddTask);
            
            Button btn_elim = new Button();
            btn_elim.Content = "Eliminar lista";
            btn_elim.Click += elim_lista;
            btn_elim.Background = new SolidColorBrush(Colors.Aquamarine);
            menu.Children.Add(btn_elim);


            //Colores
            mainL.Background = new SolidColorBrush(ColorPickerH.getColorDifferent(0));
            Button btn_selcol = new Button();
            btn_selcol.Content = "Seleccionar Color Fondo";
            btn_selcol.Click += selcol;
            btn_selcol.Background = new SolidColorBrush(Colors.Aquamarine);
            menu.Children.Add(btn_selcol);
            
            Button btn_selimg = new Button();
            btn_selimg.Content = "Seleccionar Imagen Fondo";
            btn_selimg.Click += selimg;
            btn_selimg.Background = new SolidColorBrush(Colors.Aquamarine);
            menu.Children.Add(btn_selimg);

            //Colores
            Button btn_selcol_text = new Button();
            btn_selcol_text.Content = "Seleccionar Color Texto";
            btn_selcol_text.Click += selcolText;
            btn_selcol_text.Background = new SolidColorBrush(Colors.Aquamarine);
            btn_selcol_text.Name = stackP.Children.Count + "";
            menu.Children.Add(btn_selcol_text);

            menu.Orientation = Orientation.Horizontal;
            mainL.Items.Add(menu);
            //scr.Content = mainL;
            //scr.HorizontalAlignment = HorizontalAlignment.Stretch;
            //scr.HorizontalScrollMode = ScrollMode.Enabled;

            ListCreated.lists.Add(mainL);
            stackP.Children.Add(mainL);
        }

        private void selcolText(object sender, RoutedEventArgs e){
            ColorPicker.ColorPicker c = new ColorPicker.ColorPicker();
            c.PointerPressed += selcolFin;
            c.SelectedColorChanged += colorTextSelected;
            ColorPickerH.sd = c.SelectedColor;
            c.Width = 500;
            ((ListView)((StackPanel)((Button)sender).Parent).Parent).Items.Add(c);
        }

        private void colorTextSelected(object sender, System.EventArgs e){
            if (((ColorPicker.ColorPicker)sender).Parent != null)
            {
                ListView parent_l = (ListView)((ColorPicker.ColorPicker)sender).Parent;
                TextBox title = ((TextBox)parent_l.Items.ElementAt(0));
                title.Foreground = ((ColorPicker.ColorPicker)sender).SelectedColor;
                StackPanel stck = ((StackPanel)parent_l.Items.ElementAt(1));
                DatePicker dp = (DatePicker)stck.Children.ElementAt(1);
                dp.Foreground = ((ColorPicker.ColorPicker)sender).SelectedColor;
                TimePicker tp = (TimePicker)stck.Children.ElementAt(2);
                tp.Foreground = ((ColorPicker.ColorPicker)sender).SelectedColor;
                
                ListView tasks = ((ListView)parent_l.Items.ElementAt(2));
                for (int i = 0; i < tasks.Items.Count; i++){
                    CheckBox cb = (CheckBox)tasks.Items.ElementAt(i);
                    TextBox tb = (TextBox)cb.Content;
                    tb.Foreground = ((ColorPicker.ColorPicker)sender).SelectedColor;
                }
            }
        }

        private async void createNotificationTask(object sender, RoutedEventArgs e){
            StackPanel parent_stck = (StackPanel)((Button)sender).Parent;
            DatePicker dp = (DatePicker)parent_stck.Children.ElementAt(1);
            TimePicker tp = (TimePicker)parent_stck.Children.ElementAt(2);

            DateTimeOffset selD = dp.Date;
            TimeSpan tm = tp.Time;

            // Set up the notification text.
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList strings = toastXml.GetElementsByTagName("text");
            ListView parent_lv = (ListView)parent_stck.Parent;
            TextBox tb_title = (TextBox)parent_lv.Items.ElementAt(0);
            String texto = tb_title.Text;
            strings[0].AppendChild(toastXml.CreateTextNode(texto));
            strings[1].AppendChild(toastXml.CreateTextNode("Fecha: " + selD));

            // Create the toast notification object.
            selD = selD.AddSeconds(30);
            try{
                DateTimeOffset d = new DateTimeOffset();
                d = d.AddDays(selD.Day);
                d = d.AddMonths(selD.Month);
                d = d.AddYears(selD.Year);
                d = d.AddHours(tm.Hours);
                d = d.AddMinutes(tm.Minutes);
                d = d.AddSeconds(tm.Seconds + 30);
                ScheduledToastNotification toast = new ScheduledToastNotification(toastXml, d);
                toast.Id = "h" + new Random().Next();
                // Add to the schedule.
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);

            } catch (Exception excep){
                MessageDialog showDialog = new MessageDialog("Introduce una fecha futura");

                showDialog.Commands.Add(new UICommand("Vale") { Id = 0 });

                showDialog.DefaultCommandIndex = 0;

                var result = await showDialog.ShowAsync();
            }
        }
        /*
        private void time_chng(object sender, TimePickerValueChangedEventArgs e){
            TimeSpan tm = ((TimePicker)sender).Time;
            // Set up the notification text.
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList strings = toastXml.GetElementsByTagName("text");
            StackPanel parent_stck = (StackPanel)((TimePicker)sender).Parent;
            ListView parent_lv = (ListView)parent_stck.Parent;
            TextBox tb_title = (TextBox)parent_lv.Items.ElementAt(0);
            String texto = tb_title.Text;
            strings[0].AppendChild(toastXml.CreateTextNode(texto));
            strings[1].AppendChild(toastXml.CreateTextNode("Fecha: " + tm));
            /*DateTimeOffset dt = new Windows.Globalization.Calendar().GetDateTime();
              dt += tm;*
            // Create the toast notification object.
            //ScheduledToastNotification toast = new ScheduledToastNotification(toastXml, dt);
            try{
                ScheduledToastNotification toast = new ScheduledToastNotification(toastXml, new Windows.Globalization.Calendar().GetDateTime(), tm, 5);
                // Add to the schedule.
                toast.Id = "h" + new Random().Next();
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            }catch(Exception excep){
            }
        }

        private void fecha_chng(object sender, DatePickerValueChangedEventArgs e){
            DateTimeOffset selD = ((DatePicker)sender).Date;

            // Set up the notification text.
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList strings = toastXml.GetElementsByTagName("text");
            StackPanel parent_stck = (StackPanel)((DatePicker)sender).Parent;
            ListView parent_lv = (ListView)parent_stck.Parent;
            TextBox tb_title = (TextBox)parent_lv.Items.ElementAt(0);
            String texto = tb_title.Text;
            strings[0].AppendChild(toastXml.CreateTextNode(texto));
            strings[1].AppendChild(toastXml.CreateTextNode("Fecha: " + selD));

            // Create the toast notification object.
            selD=selD.AddSeconds(30);
            try{
                ScheduledToastNotification toast = new ScheduledToastNotification(toastXml, selD);
                toast.Id = "h" + new Random().Next();
                // Add to the schedule.
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            }catch(Exception excep){
            }
}
*/

        private async void selimg(object sender, RoutedEventArgs e){
            Windows.Storage.Pickers.FileOpenPicker f = new Windows.Storage.Pickers.FileOpenPicker();
            f.FileTypeFilter.Add(".jpg");
            f.FileTypeFilter.Add(".jpeg");
            f.FileTypeFilter.Add(".png");
            Windows.Storage.StorageFile file = await f.PickSingleFileAsync();

            if (file != null){
                var stream = await file.OpenReadAsync();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = bitmapImage;
                ((ListView)((StackPanel)((Button)sender).Parent).Parent).Background = ib;
            }
        }

        private void selcol(object sender, RoutedEventArgs e){
            ColorPicker.ColorPicker c = new ColorPicker.ColorPicker();
            c.PointerPressed += selcolFin;
            c.SelectedColorChanged += a;
            ColorPickerH.sd = c.SelectedColor;
            c.Width = 500;
            ((ListView)((StackPanel)((Button) sender).Parent).Parent).Items.Add(c);
        }

        private void selcolFin(object sender, RoutedEventArgs e){
            if (!ColorPickerH.first){
                ((ListView)((ColorPicker.ColorPicker)sender).Parent).Items.Remove(sender);
                ColorPickerH.first = true;
            } else{
                ColorPickerH.first = false;
            }
        }

        void a(object c, System.EventArgs e){
            if (c != null){
                if (((ColorPicker.ColorPicker)c) != null){
                    if (((ColorPicker.ColorPicker)c).Parent != null){
                        ((ListView)((ColorPicker.ColorPicker)c).Parent).Background = ((ColorPicker.ColorPicker)c).SelectedColor;
                    }
                }
            }
        }
    }
}
