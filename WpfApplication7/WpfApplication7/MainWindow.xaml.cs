using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace WpfApplication7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = null, timer1 = null;
        private int payingway; // 1 - оплата наличными 2 - оплата бск 3 - оплата кредиткой
        public int ta, tb, tc, td; //таймера на видео;
        OptionsMessages opt = new OptionsMessages();
        public int trigger = 0;
        TimeSpan tsa, tsb, tsc, tsd;
        bool trigout = false;
        bool cashing = false; //есть возможность внести деньги в ящик или нет  
        public MoneyBox moneybox;
        public BSCard bscard = new BSCard();
        public Roadcontrol roadcontrol;
        public VehQueue queue;
        public int payvalue = 0; //значение текущей стоимости оплаты
        Banknote b;
        public int cash = 0; //сдача
        public Props props = new Props();
        public string stimer = "HH mm";
        public bool cardcheck = true;
        public bool authorisat = false;
        public MainWindow()
        {
            InitializeComponent();
            roadcontrol = new Roadcontrol();
            moneybox = new MoneyBox();
            queue = new VehQueue();
            props.ReadXml();
            messages.Visibility = Visibility.Hidden;
            thumb2.Visibility = Visibility.Hidden;
            DriverMes.Visibility = Visibility.Hidden;
            thumb3.Visibility = Visibility.Hidden;
            mb.Content = moneybox.checkMoney();
            countlabel.Content = queue.Count().ToString();
            countlabel1.Content = queue.Count().ToString();
            options.Visibility = Visibility.Visible;
        }
        //таймер часы
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void timerStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
            timer1 = new DispatcherTimer();
            timer1.Tick += new EventHandler(timerTick1);
            timer1.Interval = new TimeSpan(0, 0, 0, 1);
            timer1.Start();
        }
        private void timerTick(object sender, EventArgs e)
        {
            AudioControl();
            label7.Content = opt.MoneyIntToStr(moneybox.checkRealMoney());
            if (stimer == "HH mm") stimer = "HH:mm";
            else stimer = "HH mm";
            timelabel.Content = DateTime.Now.ToString(stimer, System.Globalization.CultureInfo.InvariantCulture);
            datelabel.Content = DateTime.Now.ToString("d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture) + "\n" + DateTime.Now.ToString("dddd", System.Globalization.CultureInfo.CreateSpecificCulture("ru"));
            timelabel1.Content = DateTime.Now.ToString(stimer, System.Globalization.CultureInfo.InvariantCulture);
            datelabel1.Content = DateTime.Now.ToString("d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture) + "\n" + DateTime.Now.ToString("dddd", System.Globalization.CultureInfo.CreateSpecificCulture("ru"));
            IsAddTs();
            
        }
        private void timerTick1(object sender, EventArgs e)
        {
            try { VideoCurControl(); }
            catch { }
            
        }
        //панель calculate на вкладке авторизации
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void button_backClick(object sender, RoutedEventArgs e)
        {
            if ((enter_pass.Text.Length > 0) && (enter_pass.Text != "Введите пароль доступа"))
            {
                enter_pass.Text = enter_pass.Text.Remove(enter_pass.Text.Length - 1, 1);
            }
        }
        private void buttonNUM_Click(object sender, RoutedEventArgs e)
        {

            string s = sender.ToString();
            int a = s.Length;
            if (enter_pass.Text == "Введите пароль доступа") enter_pass.Text = s[a - 1].ToString();
            else enter_pass.Text += s[a - 1];

        }
        private void buttonNUM(object sender, RoutedEventArgs e)
        {
            if ((!enter_pass.Text.Contains(".")) && (enter_pass.Text != "Введите пароль доступа"))
            {
                if (enter_pass.Text.Length == 0) enter_pass.Text += "0";
                enter_pass.Text += ".";
            }
        }
        private void enter_pass_GotFocus(object sender, RoutedEventArgs e)
        {
            enter_pass.Text = "";
        }
        private void enter_pass_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (enter_pass.Text == "") enter_pass.Text = "Введите пароль доступа";
        }
        //панель calculate на вкладке внесения денег
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void button_backClick1(object sender, RoutedEventArgs e)
        {
            if ((calculate_box1.Text.Length > 0))
            {
                calculate_box1.Text = calculate_box1.Text.Remove(calculate_box1.Text.Length - 1, 1);
            }
        }
        private void buttonNUM_Click1(object sender, RoutedEventArgs e)
        {
            if (calculate_box1.Text == "0") calculate_box1.Text = "";
            string s = sender.ToString();
            int a = s.Length;
            calculate_box1.Text += s[a - 1];

        }
        private void buttonNUM1(object sender, RoutedEventArgs e)
        {
            if ((!calculate_box1.Text.Contains(".")))
            {
                if (calculate_box1.Text.Length == 0) calculate_box1.Text += "0";
                calculate_box1.Text += ".";
            }
        }
        //панель calculate на вкладке выдачи денег
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void button_backClick2(object sender, RoutedEventArgs e)
        {
            if ((calculate_box2.Text.Length > 0))
            {
                calculate_box2.Text = calculate_box2.Text.Remove(calculate_box2.Text.Length - 1, 1);
            }
        }
        private void buttonNUM_Click2(object sender, RoutedEventArgs e)
        {
            if (calculate_box2.Text == "0") calculate_box2.Text = "";
            string s = sender.ToString();
            int a = s.Length;
            calculate_box2.Text += s[a - 1];

        }
        private void buttonNUM2(object sender, RoutedEventArgs e)
        {
            if ((!calculate_box2.Text.Contains(".")))
            {
                if (calculate_box2.Text.Length == 0) calculate_box2.Text += "0";
                calculate_box2.Text += ".";
            }
        }
        //панель calculate на вкладке оплаты наличкой
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void button_backClick3(object sender, RoutedEventArgs e)
        {
            if ((calculate_box3.Text.Length > 0))
            {
                calculate_box3.Text = calculate_box3.Text.Remove(calculate_box3.Text.Length - 1, 1);
            }
        }
        private void buttonNUM_Click3(object sender, RoutedEventArgs e)
        {
            if (calculate_box3.Text == (payvalue / 100).ToString()) calculate_box3.Text = "";
            string s = sender.ToString();
            int a = s.Length;
            calculate_box3.Text += s[a - 1];
            int b = opt.MoneyStrToInt(calculate_box3.Text);
            int c = b - payvalue;
            if (c < 0) c = 0;
            cashval.Content = c / 100;

        }
        private void buttonNUM3(object sender, RoutedEventArgs e)
        {
            if ((!calculate_box3.Text.Contains(".")))
            {
                if (calculate_box3.Text.Length == 0) calculate_box3.Text += "0";
                calculate_box3.Text += ".";
            }
        }
        //панель calculate на вкладке оплаты картой
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void button_backClick4(object sender, RoutedEventArgs e)
        {
            if ((calculate_box4.Text.Length > 0))
            {
                if (calculate_box4.Text.Length != 0) calculate_box4.Text = calculate_box4.Text.Remove(calculate_box4.Text.Length - 1, 1);
            }
        }
        private void buttonNUM_Click4(object sender, RoutedEventArgs e)
        {
            string s = sender.ToString();
            int a = s.Length;
            if (calculate_box4.Text == "или введите штрих-код") calculate_box4.Text = s[a - 1].ToString();
            else calculate_box4.Text += s[a - 1];

        }
        private void buttonNUM4(object sender, RoutedEventArgs e)
        {
            if ((!calculate_box4.Text.Contains(".")))
            {
                if (calculate_box4.Text.Length == 0) calculate_box4.Text += "0";
                calculate_box4.Text += ".";
            }
        }
        private void cb4_GotFocus(object sender, RoutedEventArgs e)
        {
            calculate_box4.Text = "";
        }
        private void cb4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (calculate_box4.Text == "") calculate_box4.Text = "или введите штрих-код";
            if (calculate_box4.Text == bscard.number.ToString())
            {
                mw.Visibility = Visibility.Hidden;
                mw2.Visibility = Visibility.Visible;
                calculate_box4.Text = "или введите штрих-код";
            }
        }
        // открытие закрытие полосы. элементы
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void car_check_Click(object sender, RoutedEventArgs e)
        {
            CrossMark();
            if (roadcontrol.light != roadcontrol.gate) GateMark();
        }
        private void way_check1_Click(object sender, RoutedEventArgs e) { PaymentMark(); }
        private void way_check2_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(way_check2.GetValue(Canvas.LeftProperty)) > 0)
            {
                for (double i = 118; i >= 0; i--)
                {
                    way_check2.SetValue(Canvas.LeftProperty, i);
                    way_check2.Content = "Закрыть полосу";
                }
            }
            else for (double i = 0; i <= 118; i++)
                {
                    way_check2.SetValue(Canvas.LeftProperty, i);
                    way_check2.Content = "Открыть полосу";
                }
        }
        private void group_way_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(group_way.GetValue(Canvas.LeftProperty)) > 264)
            {
                for (double i = 364; i >= 246; i--)
                {
                    group_way.SetValue(Canvas.LeftProperty, i);
                    group_way.Content = "Закрыть выезд";
                }
            }
            else for (double i = 246; i <= 364; i++)
                {
                    group_way.SetValue(Canvas.LeftProperty, i);
                    group_way.Content = "Открыть выезд";
                }
        }
        // кнопкка Добавить ТС
        private void addTS_Click(object sender, RoutedEventArgs e)
        {
            if (queue.Count() == 0)
            {
                change_class.IsEnabled = true;
                add_scep.IsEnabled = true;
                deleteTS.IsEnabled = true;
                Work.IsSelected = true;           

            }
            queue.Add();          
            class_but.Content = queue.GetClassZero();
            countlabel.Content = queue.Count().ToString();
            countlabel1.Content = queue.Count().ToString();
            ta = opt.timers(queue.GetClassZero(), 0, queue.GetNumZero());
            tb = opt.timers(queue.GetClassZero(), 1, queue.GetNumZero());
            tc = opt.timers(queue.GetClassZero(), 2, queue.GetNumZero());
            td = opt.timers(queue.GetClassZero(), 3, queue.GetNumZero());

        }
        private void calculate_box1_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void gatetumb_Click(object sender, RoutedEventArgs e)
        {
            GateMark();
        }
        private void greenlight_Click(object sender, RoutedEventArgs e) { CrossMark(); }
        private void getpayment_Click(object sender, RoutedEventArgs e) { PaymentMark(); }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        // перемещение карты
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void thumb1_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }
        private void thumb1_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            thumb1.Margin = new Thickness(thumb1.Margin.Left + e.HorizontalChange, thumb1.Margin.Top + e.VerticalChange, thumb1.Margin.Right - e.HorizontalChange, thumb1.Margin.Bottom - e.VerticalChange);
        }
        private void thumb1_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Cardcheck();

        }
        public void Cardcheck()
        {
            double up = bskimg.Margin.Top - thumb1.Height;
            double down = bskimg.Margin.Top + bskimg.Height;
            double left = bskimg.Margin.Left - thumb1.Width;
            double right = bskimg.Margin.Left + bskimg.Width;

            if (cardcheck == true)
            {
                // Первое приложение БСК
                /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

                if ((thumb1.Margin.Left > left) && (thumb1.Margin.Left < right) && (thumb1.Margin.Top > up) && (thumb1.Margin.Top < down))
                {
                    Autorisation.IsSelected = true;
                    thumb1.Margin = new Thickness(10, 215, 0, 0);
                    cardcheck = false;

                }
                else
                {
                    thumb1.Margin = new Thickness(10, 215, 0, 0);

                }

            }
            else
            {
                // повторное приложение БСК. вход в технологическую панель
                /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

                if ((authorisat == true) && ((thumb1.Margin.Left > left) && (thumb1.Margin.Left < right) && (thumb1.Margin.Top > up) && (thumb1.Margin.Top < down)))
                {
                    RightPanel.IsSelected = true;
                    TechPanel.IsSelected = true;
                }
                thumb1.Margin = new Thickness(10, 215, 0, 0);
            }
        }
        // запуск видео на всех медиаплеерах проекта
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void VideoControl()
        {
            string s = @"123.mp4";
            mediaElement.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement.Play();            
            mediaElement1.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement1.Play();
            mediaElement2.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement2.Play();
            mediaElement3.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement3.Play();
            mediaElement4.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement4.Play();
            mediaElement5.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement5.Play();
            mediaElement6.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement6.Play();
            mediaElement7.Source = new Uri(s, UriKind.RelativeOrAbsolute); mediaElement7.Play();

        }
        // контроль за звуком - звук работает только на той вкладке, что в данный момент открыта (если на ней есть видео)
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void AudioControl()
        {
            mediaElement.IsMuted = true;
            mediaElement1.IsMuted = true;
            mediaElement2.IsMuted = true;
            mediaElement3.IsMuted = true;
            mediaElement4.IsMuted = true;
            mediaElement5.IsMuted = true;
            mediaElement6.IsMuted = true;
            mediaElement7.IsMuted = true;            
            if ((Waiting.IsSelected == true) && (RightPanel.IsSelected == true)) mediaElement.IsMuted = false;
            if ((MainWind.IsSelected == true) && (LeftPanel.IsSelected == true)) mediaElement1.IsMuted = false;
            if ((contrWay.IsSelected == true) && (RightPanel.IsSelected == true)) mediaElement2.IsMuted = false;
            if ((Work.IsSelected == true) && (LeftPanel.IsSelected == true)) mediaElement3.IsMuted = false;
            if ((SpecPanel.IsSelected == true) && (LeftPanel.IsSelected == true)) mediaElement4.IsMuted = false;
            if ((contrWay1.IsSelected == true) && (RightPanel.IsSelected == true)) mediaElement5.IsMuted = false;
            if ((Classpanel.IsSelected == true) && (LeftPanel.IsSelected == true)) mediaElement6.IsMuted = false;
            if ((Finish.IsSelected == true) && (LeftPanel.IsSelected == true)) mediaElement7.IsMuted = false;
        }
        // Кнопка отмена, возврат на экран авторизации
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void button_exit_Click(object sender, RoutedEventArgs e)
        {
            Waiting.IsSelected = true;
            cardcheck = true;
        }
        // Переход на вкладку напоминания
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void button_enter_Click(object sender, RoutedEventArgs e)
        {
            Warning.IsSelected = true;
        }
        // Переход на основной экран
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            return_to_main.IsEnabled = true;
            LeftPanel.IsSelected = true;
            MainWind.IsSelected = true;
            authorisat = true;

        }
        // Кнопка отмена, возврат на экран авторизации
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Autorisation.IsSelected = true;
        }
        // Кнопка завершения работы
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void EndWork_Click(object sender, RoutedEventArgs e)
        {
            cardcheck = true;
            authorisat = false;
            Waiting.IsSelected = true;
            thumb1.Margin = new Thickness(10, 215, 0, 0);
            VideoControl();
            return_to_main.IsEnabled = false;
        }
        // Внесение/выдача денег
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void Inputmoney_Click(object sender, RoutedEventArgs e)
        {
            InsertMoney.IsSelected = true;

        }
        private void PayMoney_Click(object sender, RoutedEventArgs e)
        {
            PayingMoney.IsSelected = true;
        }
        // выплата денег
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void Input_money_Copy_Click(object sender, RoutedEventArgs e)
        {
            int money = opt.MoneyStrToInt(calculate_box2.Text.ToString());
            moneybox.takeMoney(money);
            mb.Content = opt.MoneyIntToStr(moneybox.checkMoney());


            calculate_box2.Text = "0";
            RightPanel.IsSelected = true;
            TechPanel.IsSelected = true;
        }
        // внесение денег
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void Input_money_Click(object sender, RoutedEventArgs e)
        {

            int money = opt.MoneyStrToInt(calculate_box1.Text.ToString());
            moneybox.inputMoney(money);
            mb.Content = opt.MoneyIntToStr(moneybox.checkMoney());
            calculate_box1.Text = "0";
            RightPanel.IsSelected = true;
            TechPanel.IsSelected = true;
        }
        // технологическая панель
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void options_work_Click(object sender, RoutedEventArgs e)
        {
            RightPanel.IsSelected = true;
            if (rb1.IsChecked == true) contrWay.IsSelected = true;
            else contrWay1.IsSelected = true;
        }
        // возврат на главное меню
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void return_to_main_Click(object sender, RoutedEventArgs e)
        {
            LeftPanel.IsSelected = true;
            MainWind.IsSelected = true;
            authorisat = true;
            if (queue.Count() != 0) Work.IsSelected = true;
            
        }
        // изменение класса ТС
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void class_but_1_Click(object sender, RoutedEventArgs e)
        {
            queue.SetClassZero(1);
            Work.IsSelected = true;
            class_but.Content = queue.GetClassZero();

        }
        private void class_but_2_Click(object sender, RoutedEventArgs e)
        {
            queue.SetClassZero(2);
            Work.IsSelected = true;
            class_but.Content = queue.GetClassZero();
        }
        private void class_but_3_Click(object sender, RoutedEventArgs e)
        {
            queue.SetClassZero(3);
            Work.IsSelected = true;
            class_but.Content = queue.GetClassZero();
        }
        private void class_but_4_Click(object sender, RoutedEventArgs e)
        {
            queue.SetClassZero(4);
            Work.IsSelected = true;
            class_but.Content = queue.GetClassZero();
        }
        // контроллер зоны въезда
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void CrossMark()
        {
            if (roadcontrol.light == true)
            {
                roadcontrol.redLight();
                for (double i = 0; i <= 118; i++) car_check.SetValue(Canvas.LeftProperty, i);
                _1_png.Visibility = Visibility.Visible;
                _11_png.Visibility = Visibility.Collapsed;
                noway_png.Visibility = Visibility.Visible;
                yesway_png.Visibility = Visibility.Collapsed;
                greencross_png.Visibility = Visibility.Collapsed;
                redcross_png.Visibility = Visibility.Visible;
                car_check.Content = "Разрешить въезд";
                closeway.Content = "Открыть полосу";

            }
            else
            {
                for (double i = 118; i >= 0; i--) car_check.SetValue(Canvas.LeftProperty, i);
                roadcontrol.greenLight();
                greencross_png.Visibility = Visibility.Visible;
                redcross_png.Visibility = Visibility.Collapsed;
                _1_png.Visibility = Visibility.Collapsed;
                _11_png.Visibility = Visibility.Visible;
                noway_png.Visibility = Visibility.Collapsed;
                yesway_png.Visibility = Visibility.Visible;
                car_check.Content = "Запретить въезд";
                closeway.Content = "Закрыть полосу";
            }

        }
        // контроллер режима работы
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void PaymentMark()
        {
            switch (roadcontrol.paycheck)
            {
                case 0:
                    for (double i = 118; i >= 0; i--) way_check1.SetValue(Canvas.LeftProperty, i);
                    roadcontrol.manualPaycheck();
                    _2_png.Visibility = Visibility.Collapsed;
                    _22_png.Visibility = Visibility.Visible;
                    redX_png.Visibility = Visibility.Hidden;
                    greenX_png.Visibility = Visibility.Visible;
                    whiteroundthingnoway_png.Visibility = Visibility.Collapsed;
                    whiteroundthingyesway_png.Visibility = Visibility.Visible;
                    way_check1.Content = "Закрыть въезд";

                    break;
                case 1:
                    for (double i = 0; i <= 118; i++) way_check1.SetValue(Canvas.LeftProperty, i);
                    roadcontrol.noPaycheck();
                    _2_png.Visibility = Visibility.Visible;
                    _22_png.Visibility = Visibility.Collapsed;
                    redX_png.Visibility = Visibility.Visible;
                    greenX_png.Visibility = Visibility.Hidden;
                    whiteroundthingnoway_png.Visibility = Visibility.Visible;
                    whiteroundthingyesway_png.Visibility = Visibility.Collapsed;
                    way_check1.Content = "Открыть въезд";

                    break;
            }


        }
        // контроллер шлагбаума
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void GateMark()
        {
            if (roadcontrol.gate == true)
            {
                greenway_png.Visibility = Visibility.Hidden;
                redway_png.Visibility = Visibility.Visible;
                roadcontrol.closeGate();
                close_road_thing_png.Visibility = Visibility.Visible;
                open_road_thing_png.Visibility = Visibility.Hidden;
            }
            else
            {
                greenway_png.Visibility = Visibility.Visible;
                redway_png.Visibility = Visibility.Hidden;
                close_road_thing_png.Visibility = Visibility.Hidden;
                open_road_thing_png.Visibility = Visibility.Visible;

                roadcontrol.openGate();
            }


        }

        // кнопка Удалить ТС
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void deleteTS_Click(object sender, RoutedEventArgs e)
        {
            DeleteTs();
            trigout = false;
            trigger = 0;
        }
        public void DeleteTs()
        {
            queue.Delete();      
            
            countlabel.Content = queue.Count().ToString();
            countlabel1.Content = queue.Count().ToString();
            if (queue.Count() != 0)
            {
                class_but.Content = queue.GetClassZero();
                ta = opt.timers(queue.GetClassZero(), 0, queue.GetNumZero());
                tb = opt.timers(queue.GetClassZero(), 1, queue.GetNumZero());
                tc = opt.timers(queue.GetClassZero(), 2, queue.GetNumZero());
                td = opt.timers(queue.GetClassZero(), 3, queue.GetNumZero());
            }
            else
            {
                change_class.IsEnabled = false;
                add_scep.IsEnabled = false;
                deleteTS.IsEnabled = false;
                MainWind.IsSelected = true;

            }
        }
        // функция скрытия панелек. вынесена отдельно, чтобы не перегружать код
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void TechPanels()
        {
            RightPanel.Visibility = Visibility.Hidden;
            LeftPanel.Visibility = Visibility.Hidden;
            MainWind.Visibility = Visibility.Hidden;
            Work.Visibility = Visibility.Hidden;
            Pay.Visibility = Visibility.Hidden;
            Classpanel.Visibility = Visibility.Hidden;
            SpecPanel.Visibility = Visibility.Hidden;
            Waiting.Visibility = Visibility.Hidden;
            Autorisation.Visibility = Visibility.Hidden;
            Warning.Visibility = Visibility.Hidden;
            TechPanel.Visibility = Visibility.Hidden;
            InsertMoney.Visibility = Visibility.Hidden;
            contrWay.Visibility = Visibility.Hidden;
            contrWay1.Visibility = Visibility.Hidden;
            PayingMoney.Visibility = Visibility.Hidden;
            Finish.Visibility = Visibility.Hidden;
        }
        // функция открытия панели. Новые версии АРМа
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void closeway_Click(object sender, RoutedEventArgs e)
        {
            if (closeway.Content.ToString() == "Открыть полосу")
            {
                if (roadcontrol.light == false) CrossMark();
                if (roadcontrol.gate == false) GateMark();
                if (roadcontrol.paycheck == 0) PaymentMark();
            }
            else
            {
                if (roadcontrol.light == true) CrossMark();
                if (roadcontrol.gate == true) GateMark();
                if (roadcontrol.paycheck == 1) PaymentMark();
            }
        }
        // функция проверки добавления ТС
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void IsAddTs()
        {
            if ((roadcontrol.light == true) && (roadcontrol.gate == true) && (roadcontrol.paycheck != 0)) addTS.IsEnabled = true;
            else addTS.IsEnabled = false;
        }
        // Кнопка запуска программы
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void start(object sender, RoutedEventArgs e)
        {
            options.Visibility = Visibility.Hidden;
            timerStart();
            VideoControl();
            TechPanels();
        }
        // кнопки опций
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void options_but_Click(object sender, RoutedEventArgs e)
        {
            optContr.Visibility = Visibility.Visible;
            Opt1.IsSelected = true;
            props.ReadXml();
            pay1stclassday.Text = opt.MoneyIntToStr(props.Fields.firstclassday);
            pay2stclassday.Text = opt.MoneyIntToStr(props.Fields.secondclassday);
            pay3stclassday.Text = opt.MoneyIntToStr(props.Fields.thirdclassday);
            pay4stclassday.Text = opt.MoneyIntToStr(props.Fields.forthclassday);
            validations.Text = opt.MoneyIntToStr(props.Fields.validation);
        }
        private void about_Click(object sender, RoutedEventArgs e)
        {
            optContr.Visibility = Visibility.Visible;
            Opt2.IsSelected = true;
        }
        private void OKbut_Click(object sender, RoutedEventArgs e)
        {
            optContr.Visibility = Visibility.Hidden;
            props.Fields.firstclassday = opt.MoneyStrToInt(pay1stclassday.Text.ToString());
            props.Fields.secondclassday = opt.MoneyStrToInt(pay2stclassday.Text.ToString());
            props.Fields.thirdclassday = opt.MoneyStrToInt(pay3stclassday.Text.ToString());
            props.Fields.forthclassday = opt.MoneyStrToInt(pay4stclassday.Text.ToString());
            props.Fields.validation = opt.MoneyStrToInt(validations.Text.ToString());
            props.WriteXml();
        }
        // настройка размера кнопочек класса автомобиля
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void class_but_Click(object sender, RoutedEventArgs e)
        {
            Classpanel.IsSelected = true;
            switch (queue.GetClassZero() - 1)
            {
                case 0:
                    class_but_1.Margin = new Thickness(46, 304, 480, 193);
                    class_but_2.Margin = new Thickness(168, 320, 387, 210);
                    class_but_3.Margin = new Thickness(278, 320, 277, 210);
                    class_but_4.Margin = new Thickness(384, 320, 171, 210);
                    class_but_spec.Margin = new Thickness(495, 320, 60, 210);
                    class_but_1.FontSize = 70;
                    class_but_2.FontSize = 45;
                    class_but_3.FontSize = 45;
                    class_but_4.FontSize = 45;
                    class_but_spec.FontSize = 45;
                    break;
                case 1:
                    class_but_1.Margin = new Thickness(59, 320, 496, 210);
                    class_but_2.Margin = new Thickness(153, 303, 373, 194);
                    class_but_3.Margin = new Thickness(278, 320, 277, 210);
                    class_but_4.Margin = new Thickness(384, 320, 171, 210);
                    class_but_spec.Margin = new Thickness(495, 320, 60, 210);
                    class_but_1.FontSize = 45;
                    class_but_2.FontSize = 70;
                    class_but_3.FontSize = 45;
                    class_but_4.FontSize = 45;
                    class_but_spec.FontSize = 45;
                    break;
                case 2:
                    class_but_1.Margin = new Thickness(59, 320, 496, 210);
                    class_but_2.Margin = new Thickness(168, 320, 387, 210);
                    class_but_3.Margin = new Thickness(262, 303, 264, 194);
                    class_but_4.Margin = new Thickness(384, 320, 171, 210);
                    class_but_spec.Margin = new Thickness(495, 320, 60, 210);
                    class_but_1.FontSize = 45;
                    class_but_2.FontSize = 45;
                    class_but_3.FontSize = 70;
                    class_but_4.FontSize = 45;
                    class_but_spec.FontSize = 45;
                    break;
                case 3:
                    class_but_1.Margin = new Thickness(59, 320, 496, 210);
                    class_but_2.Margin = new Thickness(168, 320, 387, 210);
                    class_but_3.Margin = new Thickness(278, 320, 277, 210);
                    class_but_4.Margin = new Thickness(369, 303, 157, 194);
                    class_but_spec.Margin = new Thickness(495, 320, 60, 210);
                    class_but_1.FontSize = 45;
                    class_but_2.FontSize = 45;
                    class_but_3.FontSize = 45;
                    class_but_4.FontSize = 70;
                    class_but_spec.FontSize = 45;
                    break;
                case 4:
                    class_but_1.Margin = new Thickness(59, 320, 496, 210);
                    class_but_2.Margin = new Thickness(168, 320, 387, 210);
                    class_but_3.Margin = new Thickness(278, 320, 277, 210);
                    class_but_4.Margin = new Thickness(384, 320, 171, 210);
                    class_but_spec.Margin = new Thickness(479, 303, 47, 194);
                    class_but_1.FontSize = 45;
                    class_but_2.FontSize = 45;
                    class_but_3.FontSize = 45;
                    class_but_4.FontSize = 45;
                    class_but_spec.FontSize = 60;
                    break;

            }
        }
        // спец транспорт
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void class_but_spec_Click(object sender, RoutedEventArgs e)
        {
            SpecPanel.IsSelected = true;
        }
        private void class_but1_Click(object sender, RoutedEventArgs e)
        {
            class_but_1.Margin = new Thickness(59, 320, 496, 210);
            class_but_2.Margin = new Thickness(168, 320, 387, 210);
            class_but_3.Margin = new Thickness(278, 320, 277, 210);
            class_but_4.Margin = new Thickness(384, 320, 171, 210);
            class_but_spec.Margin = new Thickness(479, 303, 47, 194);
            class_but_1.FontSize = 45;
            class_but_2.FontSize = 45;
            class_but_3.FontSize = 45;
            class_but_4.FontSize = 45;
            class_but_spec.FontSize = 60;
            Classpanel.IsSelected = true;
        }
        // изменение класса
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void change_class_Click(object sender, RoutedEventArgs e)
        {
            Classpanel.IsSelected = true;
        }
        // подтверждение класса/генерация денег
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void accept_class_Click(object sender, RoutedEventArgs e)
        {
            Pay.IsSelected = true;
            Random r = new Random(DateTime.Now.Millisecond);
            payingway = r.Next(2) + 1;
            switch (payingway)
            {
                case 0:
                    break;
                case 1:
                    MoneyCreate();
                    break;
                case 2:
                    BscCreate();
                    break;

            }
            
            
            
        }
        // генерация банкноты
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void MoneyCreate()
        {
            SetPayValue();
            thumb2.Visibility = Visibility.Visible;
            b = new Banknote(payvalue, props.Fields.validation);
            mb1.Content = opt.MoneyIntToStr(b.nominal);
            cashval.Content = "0";
            Nal.IsSelected = true;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(@"Images\money.png", UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            card.Source = src;
        }
        private void BscCreate()
        {
            SetPayValue();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(@"Images\card.png", UriKind.Relative);            
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            card.Source = src;
            thumb2.Visibility = Visibility.Visible;
            bscard = new BSCard();
            Card.IsSelected = true;
            mb1.Content = bscard.number.ToString();
            balanseCD.Content = bscard.value/100;
            numberCD.Content = bscard.number.ToString();
            dateCD.Content = bscard.date.ToString("d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            dateOutCD.Content = bscard.dateOut.ToString("d.M.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            cashval.Content = "0";
        }
        // движение налички
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void thumb2_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }
        private void thumb2_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

            thumb2.Margin = new Thickness(thumb2.Margin.Left + e.HorizontalChange, thumb2.Margin.Top + e.VerticalChange, thumb2.Margin.Right - e.HorizontalChange, thumb2.Margin.Bottom - e.VerticalChange);
        }
        private void thumb2_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            switch (payingway)
            {
                case 0:
                    break;
                case 1:
                    {
                        TestingMoneyValidate();
                        PayingMoneybyDriver();
                    }
                    break;
                case 2:
                    {
                        PayingBscDriver();
                    }
                    break;
            }


        }
        public void PayingBscDriver()
        {
            double up = ScanTool.Margin.Top - thumb2.Height;
            double down = ScanTool.Margin.Top + ScanTool.Height;
            double left = ScanTool.Margin.Left - thumb2.Width;
            double right = ScanTool.Margin.Left + ScanTool.Width;
            if ((thumb2.Margin.Left > left) && (thumb2.Margin.Left < right) && (thumb2.Margin.Top > up) && (thumb2.Margin.Top < down)&&(bscard.trigger == true))
            {
                mw.Visibility = Visibility.Hidden;
                mw2.Visibility = Visibility.Visible;
            }
            thumb2.Margin = new Thickness(42, 36, 1114, 547);

        }
        // оплата водителем наличкой
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void PayingMoneybyDriver()
        {
            double up = MoneyBox.Margin.Top - thumb2.Height;
            double down = MoneyBox.Margin.Top + MoneyBox.Height;
            double left = MoneyBox.Margin.Left - thumb2.Width;
            double right = MoneyBox.Margin.Left + MoneyBox.Width;
            if (cashing == true)
            {
                if ((thumb2.Margin.Left > left) && (thumb2.Margin.Left < right) && (thumb2.Margin.Top > up) && (thumb2.Margin.Top < down))
                {
                    Autorisation.IsSelected = true;
                    thumb2.Margin = new Thickness(42, 36, 1114, 547);
                    thumb2.Visibility = Visibility.Hidden;
                    moneybox.addMoney(b);
                    mb.Content = opt.MoneyIntToStr(moneybox.checkMoney());
                    truefalse.Visibility = Visibility.Hidden;
                    cashing = false;
                }
                else thumb2.Margin = new Thickness(322, 525, 843, 58);
            }
            else thumb2.Margin = new Thickness(322, 525, 843, 58);
        }
        // проверка денег на подлинность
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void TestingMoneyValidate()
        {
            double up = moneycheckbox.Margin.Top - thumb2.Height;
            double down = moneycheckbox.Margin.Top + moneycheckbox.Height;
            double left = moneycheckbox.Margin.Left - thumb2.Width;
            double right = moneycheckbox.Margin.Left + moneycheckbox.Width;
            if ((thumb2.Margin.Left > left) && (thumb2.Margin.Left < right) && (thumb2.Margin.Top > up) && (thumb2.Margin.Top < down))
            {
                thumb2.Margin = new Thickness(42, 36, 1114, 547);
                truefalse.Visibility = Visibility.Visible;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                if (b.IsFake == true) src.UriSource = new Uri(@"Images\true.png", UriKind.Relative);
                else src.UriSource = new Uri("images/false.png", UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                truefalse.Source = src;
            }
        }
        // мусор, потом удалить
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void calculate_box1_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        // кнопочка появления БСК
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void card_cash_png_MouseEnter(object sender, MouseEventArgs e)
        {
            card_cash_png.Visibility = Visibility.Hidden;
            thumb1.Visibility = Visibility.Visible;
        }
        private void thumb1_MouseLeave(object sender, MouseEventArgs e)
        {
            card_cash_png.Visibility = Visibility.Visible;
            thumb1.Visibility = Visibility.Hidden;
        }
        // Установка стоимости оплаты
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void SetPayValue()
        {
            switch (queue.GetClassZero())
            {
                case 1:
                    payvalue = props.Fields.firstclassday;
                    break;
                case 2:
                    payvalue = props.Fields.secondclassday;
                    break;
                case 3:
                    payvalue = props.Fields.thirdclassday;
                    break;
                case 4:
                    payvalue = props.Fields.forthclassday;
                    break;

            }
            string s = opt.MoneyIntToStr(payvalue);
            moneyvalue.Content = s;
            calculate_box3.Text = (payvalue / 100).ToString();
        }
        // сдача
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void thumb3_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }
        private void thumb3_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

            thumb3.Margin = new Thickness(thumb3.Margin.Left + e.HorizontalChange, thumb3.Margin.Top + e.VerticalChange, thumb3.Margin.Right - e.HorizontalChange, thumb3.Margin.Bottom - e.VerticalChange);
        }
        private void thumb3_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            PayingMoneybyMan();

        }
        // выплата сдачи
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void PayingMoneybyMan()
        {
            double up = Driver.Margin.Top - thumb3.Height;
            double down = Driver.Margin.Top + Driver.Height;
            double left = Driver.Margin.Left - thumb3.Width;
            double right = Driver.Margin.Left + Driver.Width;
            if ((thumb3.Margin.Left > left) && (thumb3.Margin.Left < right) && (thumb3.Margin.Top > up) && (thumb3.Margin.Top < down))
            {

                thumb3.Margin = new Thickness(568, 525, 597, 58);
                thumb3.Visibility = Visibility.Hidden;
                CheckQueue();

            }
        }
        // кнопка выплатить
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void payWay_Click(object sender, RoutedEventArgs e)
        {
            
            if (payvalue <= opt.MoneyStrToInt(calculate_box3.Text))
            {
                thumb4.Visibility = Visibility.Visible;                
                Finish.IsSelected = true;
                if (thumb2.Visibility == Visibility.Visible)
                {
                    moneybox.addVizMoney(opt.MoneyStrToInt(calculate_box3.Text.ToString()));
                    thumb2.Margin = new Thickness(322, 525, 843, 58);
                    cashing = true;
                    mb.Content = opt.MoneyIntToStr(moneybox.checkMoney());
                    int mon = opt.MoneyStrToInt(calculate_box3.Text.ToString());
                    if (mon - payvalue > 0)
                    {
                        thumb3.Visibility = Visibility.Visible;
                        cash = mon - payvalue;
                        mb3.Content = opt.MoneyIntToStr(cash);
                        moneybox.takeMoney(cash);
                        mb.Content = opt.MoneyIntToStr(moneybox.checkMoney());
                    }

                }
            }
        }
        // ЧЕК
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void thumb4_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }
        private void thumb4_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

            thumb4.Margin = new Thickness(thumb4.Margin.Left + e.HorizontalChange, thumb4.Margin.Top + e.VerticalChange, thumb4.Margin.Right - e.HorizontalChange, thumb4.Margin.Bottom - e.VerticalChange);
        }
        private void thumb4_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            PrintBill();

        }
        // печать чека
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        public void PrintBill()
        {
            double up = Driver.Margin.Top - thumb4.Height;
            double down = Driver.Margin.Top + Driver.Height;
            double left = Driver.Margin.Left - thumb4.Width;
            double right = Driver.Margin.Left + Driver.Width;
            if ((thumb4.Margin.Left > left) && (thumb4.Margin.Left < right) && (thumb4.Margin.Top > up) && (thumb4.Margin.Top < down))
            {
                thumb4.Margin = new Thickness(46, 347, 1143, 175);
                thumb4.Visibility = Visibility.Hidden;
                trigger = 2;
                if (thumb3.Visibility == Visibility.Visible)
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                    int a = r.Next(10);
                    label7.Content = a;
                    if (a > 8)
                    {
                        thumb3.Margin = new Thickness(568, 525, 597, 58);
                        thumb3.Visibility = Visibility.Hidden;                        
                        moneybox.addCompMoney(opt.MoneyStrToInt(mb3.Content.ToString()));
                    }

                }
                
                CheckQueue();
                DeleteTs();
            }
        }
        // выплата подробный чек
        /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
        private void thumb4_MouseEnter(object sender, MouseEventArgs e)
        {
            messages.Visibility = Visibility.Visible;
            switch (payingway)
            {
                case 0:
                    break;
                case 1:
                    label8.Content = opt.printcash(payvalue, opt.MoneyStrToInt(calculate_box3.Text));
                    break;
                case 2:
                    label8.Content = opt.printbsccash(bscard.number, payvalue, bscard.value);
                    break;
            }

        }
        private void thumb4_MouseLeave(object sender, MouseEventArgs e)
        {
            messages.Visibility = Visibility.Hidden;
        }
        private void CheckQueue() //обновление данных при удалении ТС для оплаты
        {
            if (queue.Count() == 0)
            {
                change_class.IsEnabled = false;
                add_scep.IsEnabled = false;
                deleteTS.IsEnabled = false;
                MainWind.IsSelected = true;
            }
            else
            {
                Work.IsSelected = true;
                VideoCurControl();
                class_but.Content = queue.GetClassZero();

            }
            countlabel.Content = queue.Count().ToString();
            countlabel1.Content = queue.Count().ToString();
        }

        private void ttdBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (b.nominal < payvalue) MoneyGen.Content = "Попросить добавить денег или расплатиться другим способом";
                if (b.IsFake == false) MoneyGen.Content = "Попросить заменить купюру или расплатиться другим способом";
                messages.Visibility = Visibility.Visible;
                DriverMes.Visibility = Visibility.Visible;
            }
            catch { }
        }
        private void VideoCurControl()
        {
            
            switch (trigger)
            {
                case 0:
                    {
                        if (queue.Count() != 0)
                        {
                            if (trigout == false)
                            {
                                tsa = new TimeSpan(0, ta / 100, ta % 100);
                                tsb = new TimeSpan(0, tb / 100, tb % 100);
                                tsc = new TimeSpan(0, tc / 100, tc % 100);
                                tsd = new TimeSpan(0, td / 100, td % 100);
                                mediaElement.Position = tsa;
                                mediaElement1.Position = tsa;
                                mediaElement2.Position = tsa;
                                mediaElement3.Position = tsa;
                                mediaElement4.Position = tsa;
                                mediaElement5.Position = tsa;
                                mediaElement6.Position = tsa;
                                mediaElement7.Position = tsa;
                                trigger = 1;
                            }
                        }
                        else
                        {
                            if(mediaElement.Position >= new TimeSpan(0, 0, 6))
                            {
                                TimeSpan tszer = new TimeSpan(0, 0, 0);
                                mediaElement.Position = tszer;
                                mediaElement1.Position = tszer;
                                mediaElement2.Position = tszer;
                                mediaElement3.Position = tszer;
                                mediaElement4.Position = tszer;
                                mediaElement5.Position = tszer;
                                mediaElement6.Position = tszer;
                                mediaElement7.Position = tszer;
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        if (mediaElement.Position >= tsc)
                        {
                            mediaElement.Position = tsb;
                            mediaElement1.Position = tsb;
                            mediaElement2.Position = tsb;
                            mediaElement3.Position = tsb;
                            mediaElement4.Position = tsb;
                            mediaElement5.Position = tsb;
                            mediaElement6.Position = tsb;
                            mediaElement7.Position = tsb;
                        }
                    }
                    break;
                case 2:
                    {
                        if (mediaElement.Position >= tsd)
                        {
                            trigger = 0;
                            trigout = false;
                        }
                    }
                    break;
            }
        }        
        private void MoneyGen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (b.nominal < payvalue) MoneyGen.Content = "Попросить расплатиться наличкой";
                messages.Visibility = Visibility.Hidden;
                DriverMes.Visibility = Visibility.Hidden;
                MoneyCreate();
            }
            catch { }
        }      

        private void payWay1_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (bscard.value - payvalue >= 0)
            {
                thumb4.Visibility = Visibility.Visible;
                Finish.IsSelected = true;
                mw.Visibility = Visibility.Visible;
                mw2.Visibility = Visibility.Hidden;
                thumb2.Visibility = Visibility.Hidden;
            }
        }
    }
}
