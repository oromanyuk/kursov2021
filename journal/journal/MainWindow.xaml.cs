using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;

namespace journal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bd();
        }

        public void updateGrid() //Функция для обновления DataGrid
        {
            string connectionString = @"Data Source=DESKTOP-9DIOFRT\SQLEXPRESS;Initial Catalog=dnevnik;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString); // Подключение к БД
            conn.Open();// Открытие Соединения

            string spisok_sql = $"SELECT TOP (1000) [id],[name_uch],[klass],[name_pred],[name_prep_s],[data_note],[ocenka] FROM [dnevnik].[dbo].[uspev]"; //Зaпрос списка оценок
            SqlCommand comm3 = new SqlCommand(spisok_sql, conn); // Объект вывода запросов
            comm3.ExecuteNonQuery();
            SqlDataAdapter dataAdp = new SqlDataAdapter(comm3);
            DataTable dt = new DataTable("uspev");
            dataAdp.Fill(dt);
            spisok_stud.ItemsSource = dt.DefaultView; //Вывод в DataGrid 
        }
        public void bd()
        {
            string connectionString = @"Data Source=DESKTOP-9DIOFRT\SQLEXPRESS;Initial Catalog=dnevnik;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString); // Подключение к БД
            conn.Open();// Открытие Соединения

            string stud_sql = $"SELECT TOP (1000) [name_uch],[klass] FROM [dnevnik].[dbo].[students]"; //Зaпрос списка учеников
            SqlCommand comm = new SqlCommand(stud_sql, conn); // Объект вывода запросов
            SqlDataReader reader = comm.ExecuteReader(); // Выполнение запроса вывод информации

            while (reader.Read())
            {
                name_stud.Items.Add(reader[0].ToString().Trim(' ') + ", " + reader[1].ToString().Trim(' '));
            }
            reader.Close(); // Закрываем "чтение" первой таблицы

            string pred_sql = $"SELECT TOP (1000) [name_pred],[name_prep_s] FROM [dnevnik].[dbo].[predmet]"; //Зaпрос списка предметов
            SqlCommand comm2 = new SqlCommand(pred_sql, conn); // Объект вывода запросов
            SqlDataReader reader2 = comm2.ExecuteReader(); // Выполнение запроса вывод информации

            while (reader2.Read())
            {
                name_pred.Items.Add(reader2[0].ToString().Trim(' ') + ", " + reader2[1].ToString().Trim(' '));
            }
            reader2.Close(); // Закрываем "чтение" второй таблицы

            updateGrid();
        }
        private void save_Click(object sender, RoutedEventArgs e) //Добавление записи в бд
        {
            string stud_klass = name_stud.Text;
            string pred_prep = name_pred.Text;
            string data = datatime.Text;
            string ocen = ocenka_box.Text;
            string[] stud_klasM = stud_klass.Split(',');
            string stud = stud_klasM[0];
            string klass = stud_klasM[1].Trim(' ');
            string[] pred_prepM = pred_prep.Split(',');
            string pred = pred_prepM[0];
            string prep = pred_prepM[1].Trim(' ');
            
            string ocen_sql = $"INSERT INTO uspev (name_stud, klass, name_pred, name_prep_s, data_note, ocenka) VALUES ('{stud}', '{klass}', '{pred}', '{prep}', '{data}', '{ocen}')"; //Запрос вставить данные в таблицу 
            string connectionString = @"Data Source=DESKTOP-9DIOFRT\SQLEXPRESS;Initial Catalog=dnevnik;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString); // Подключение к БД
            conn.Open();// Открытие Соединения

            SqlCommand comm = new SqlCommand(ocen_sql, conn); // Объект вывода запросов
            comm.ExecuteNonQuery();
            name_stud.Text = "";
            name_pred.Text = "";
            datatime.Text = "";
            ocenka_box.Text = "";

            updateGrid();
        }

        private void update_Click(object sender, RoutedEventArgs e) //Обновление DataGrid
        {
            updateGrid();
        }

        private void delete_Click(object sender, RoutedEventArgs e) //Удаление записи в бд
        {
            string del_str = delete_box.Text;
            string del_sql = $"DELETE  FROM uspev WHERE id = '{del_str}'"; //Запрос удалить поле
            string connectionString = @"Data Source=DESKTOP-9DIOFRT\SQLEXPRESS;Initial Catalog=dnevnik;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString); // Подключение к БД
            conn.Open();// Открытие Соединения
            SqlCommand comm = new SqlCommand(del_sql, conn);// Объект вывода запросов
            comm.ExecuteNonQuery();
            delete_box.Clear();

            updateGrid();
        }
    }
}
