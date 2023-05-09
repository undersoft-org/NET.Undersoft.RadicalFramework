using EstimatR;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using TestProject.Properties;

namespace TestProject
{
    public class TestStatistics : IEstimatr
    {
        public List<SaleData> SaleDataList;
        public List<SaleData> SaleDataTestList;

        public Estimatr st;

        private EstimatorInput<EstimatorCollection, EstimatorCollection> input;

        public TestStatistics(string resourceName = "data")
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            ReadFromResourceExecelFile(resourceName);
        }

        private void ReadFromResourceExecelFile(string resourceName)
        {
            //IExcelDataReader excelDataReader = ExcelReaderFactory.CreateReader(new FileStream(@"\Resouces\data.xlsx", System.IO.FileMode.Open));

            ResourceManager rm = new ResourceManager("TestProject.Properties.Resources", typeof(Resources).Assembly);
            MemoryStream ms = new MemoryStream((byte[])rm.GetObject(resourceName));
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateReader(ms);

            int columnSize = excelDataReader.FieldCount;
            List<SaleData> inputList = new List<SaleData>();

            int i = 0;
            for (int k = 0; k < 2; k++)
            {
                inputList = new List<SaleData>();
                while (excelDataReader.Read())
                {
                    columnSize = excelDataReader.FieldCount;
                    object[] subArray = new object[columnSize];

                    for (int j = 0; j < columnSize; j++)
                    {
                        subArray[j] = excelDataReader.GetValue(j);
                    }

                    if (subArray[1] != null && subArray[1] is double)
                    {
                        SaleData saleData = new SaleData();

                        saleData.product_price = (double)subArray[1];
                        saleData.lowest_product_comp_price = (double)subArray[2];
                        saleData.price_difference = (double)subArray[3];
                        saleData.number_sold_products = (double)subArray[4];

                        inputList.Add(saleData);
                    }
                    i++;
                }

                if (excelDataReader.Name == "Data")
                {
                    SaleDataList = inputList;
                }
                if (excelDataReader.Name == "Test")
                {
                    SaleDataTestList = inputList;
                }

                if (!excelDataReader.NextResult())
                {
                    excelDataReader.Close();
                    return;
                }
            }
        }

        public Estimatr CreateEstimator(EstimatorMethod method)
        {
            if (SaleDataList != null && SaleDataList.Any() == true)
            {
                if (input == null)
                {
                    input = new EstimatorInput<EstimatorCollection, EstimatorCollection>(
                                new EstimatorCollection(SaleDataList.Select(s =>
                                    new EstimatorItem(new List<double>() { s.product_price, s.lowest_product_comp_price, s.price_difference })).ToArray()),
                                new EstimatorCollection(SaleDataList.Select(s =>
                                    new EstimatorItem(s.number_sold_products)).ToArray()));
                }
            }
            else
            {
                throw new Exception("Empty SaleDataList");
            }

            return new Estimatr(input, method);
        }

        public void RunTestStatistics(EstimatorMethod method, List<object> advancedParameters = null)
        {
            st = CreateEstimator(method);

            if (advancedParameters != null)
            {
                st.SetAdvancedParameters(advancedParameters);
            }
            st.Create();

            double estimation_error = 0;
            double estimation__relative_error = 0;
            double sum_est_rel_errpor = 0;
            double count = 0;
            double total_number_products = 0;
            double total_number_products_est = 0;

            double errorMAPE = 0;

            for (int i = 0; i < SaleDataTestList.Count; i++)
            {

                double[] x = new double[3] {SaleDataTestList[i].product_price,
                    SaleDataTestList[i].lowest_product_comp_price,
                    SaleDataTestList[i].price_difference};
                double y = SaleDataTestList[i].number_sold_products;
                double y_est = st.Evaluate(x).Vector[0];


                count++;
                total_number_products += y;
                total_number_products_est += y_est;

                errorMAPE += Math.Abs(y - y_est) / y;

                estimation_error += y - y_est;

                estimation__relative_error = (y - y_est) / y_est * 100;

                sum_est_rel_errpor += estimation__relative_error;

                Console.WriteLine(" Est: f([{0},{1}]) = {2} vs real: {3} : delta= {4} ; rel_err = {5}", x[0], x[1], y_est, y, y - y_est, estimation__relative_error);

            }

            sum_est_rel_errpor = sum_est_rel_errpor / count;

            Console.WriteLine(
                "Number estimated = " + total_number_products_est.ToString()
                + " Number real " + (total_number_products).ToString()
                + " rel_err [%] = " + (errorMAPE / count * 100).ToString());


            double[][] parameterTheta = st.GetParameters();

            if (parameterTheta != null)
            {
                Console.WriteLine("Theta: ");
                for (int k = 0; k < parameterTheta.Length; k++)
                {
                    for (int j = 0; j < parameterTheta[k].Count(); j++)
                    {
                        Console.Write("Theta[" + k.ToString() + "][" + j.ToString() + "] = " + parameterTheta[k][j].ToString());
                    }
                    Console.Write("\n");
                }
            }
        }
    }

    public class SaleData
    {
        public double product_price;
        public double lowest_product_comp_price;
        public double price_difference; //price - lowest price
        public double number_sold_products;
    }
}
