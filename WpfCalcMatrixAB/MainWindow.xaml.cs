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

namespace WpfCalcMatrixAB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
        }

        int[,] matrixA;
        int[,] matrixB;
        int[,] matrixC;

        private void MakeMatrixMultiplication(int[,] a, int[,] b, int[,] c)
        {
            int s = a.GetLength(0);

            Parallel.For(0, s, delegate (int i)
            {
                for (int j = 0; j < s; j++)
                {
                    int item = 0;

                    for (int k = 0; k < s; k++)
                    {
                        item += a[i, k] * b[k, j];
                    }

                    c[i, j] = item;
                }
            });
        }

        private void GenerateRandomMatrix(int[,] a)
        {
            int s = a.GetLength(0);

            Parallel.For(0, s, delegate (int i)
            {
                for (int j = 0; j < s; j++)
                {                    
                    a[i, j] = random.Next(0,10);
                }
            });
        }


        private static Random random = new Random();
        private void GenerateMatrix_Click(object sender, RoutedEventArgs e)
        { 
            matrixA = new int[RowA.Value??0, ColumnA.Value??0];
            GenerateRandomMatrix(matrixA);

            matrixB = new int[RowB.Value ?? 0, ColumnB.Value ?? 0];
            GenerateRandomMatrix(matrixB);

            MatrixA.ItemsSource = matrixA.ToDataTable().DefaultView;
            MatrixB.ItemsSource = matrixB.ToDataTable().DefaultView;
        }

        private void MatrixMultiplication_Click(object sender, RoutedEventArgs e)
        {            
            matrixC = new int[RowA.Value ?? 0, ColumnB.Value ?? 0];
            if (ColumnA.Value != RowB.Value)
            {
                ErrorMatrix.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMatrix.Visibility = Visibility.Collapsed;
                MakeMatrixMultiplication(matrixA, matrixB, matrixC);
            }
            MatrixC.ItemsSource = matrixC.ToDataTable().DefaultView;
        }
    }
}
