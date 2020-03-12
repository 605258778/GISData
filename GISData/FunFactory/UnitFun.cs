namespace FunFactory
{
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Display;
    using ESRI.ArcGIS.esriSystem;
    using ESRI.ArcGIS.Geometry;
    using System;
    using System.Windows.Forms;
    using Utilities;

    public class UnitFun
    {
        private const string mClassName = "FunFactory.UnitFun";
        private ErrorOpt mErrOpt = UtilFactory.GetErrorOpt();
        private IUnitConverter mESRIConvertUnits;
        private string mSubSysName = UtilFactory.GetConfigOpt().GetSystemName();

        internal UnitFun()
        {
        }

        public void bj54tojingweidu(double inx, double iny, ref string outx, ref string outy)
        {
            try
            {
                double num;
                double num2;
                int num11;
                double num12;
                double num22;
                double num23;
                double num24;
                double num25;
                double num26;
                double num27;
                double num5 = inx;
                double num6 = iny;
                int num7 = 2;
                int num8 = (int) (num5 / 1000000.0);
                double num9 = num5 - (num8 * 1000000.0);
                double num10 = num6;
                if (num9 > 500000.0)
                {
                    num9 -= 500000.0;
                    num11 = 0;
                }
                else
                {
                    num9 = 500000.0 - num9;
                    num11 = -1;
                }
                double num13 = 0.017453292519943295;
                double num14 = 1.0050517738936;
                double num15 = 0.0050623776450643;
                double num16 = 1.0624521037856E-05;
                double num17 = 2.080819597705E-08;
                double num18 = 0.0066934216229659;
                double num19 = 0.0067385254146834;
                double num20 = 6378245.0;
                double num21 = 1E-07;
                if (num7 == 1)
                {
                    num12 = ((num8 - 1) * 6) + 3;
                    num12 *= num13;
                }
                else
                {
                    num12 = num8 * 3;
                    num12 *= num13;
                }
                double a = Math.Abs(num10) / ((num14 * num20) * (1.0 - num18));
                double num28 = (num20 * (1.0 - num18)) * ((((num14 * a) - ((num15 * (Math.Sin(2.0 * a) - Math.Sin(0.0))) / 2.0)) + ((num16 * (Math.Sin(4.0 * a) - Math.Sin(0.0))) / 4.0)) - ((num17 * (Math.Sin(6.0 * a) - Math.Sin(0.0))) / 6.0));
                double num29 = num28 - num10;
                if (Math.Abs(num29) <= num21)
                {
                    num22 = num20 / Math.Sqrt(1.0 - ((num18 * Math.Sin(a)) * Math.Sin(a)));
                    num23 = (num19 * Math.Cos(a)) * Math.Cos(a);
                    num24 = Math.Tan(a);
                    num25 = Math.Abs(num9) / num22;
                    num26 = (num25 * ((1.0 - (((num25 * num25) * ((1.0 + ((2.0 * num24) * num24)) + num23)) / 6.0)) + (((((num25 * num25) * num25) * num25) * ((((5.0 + ((28.0 * num24) * num24)) + ((((24.0 * num24) * num24) * num24) * num24)) + (6.0 * num23)) + (((8.0 * num23) * num24) * num24))) / 120.0))) / Math.Cos(a);
                    if (num11 == 0)
                    {
                        num26 += num12;
                    }
                    else
                    {
                        num26 = num12 - num26;
                    }
                    num = num26;
                    num27 = ((((num9 * num9) * (1.0 + num23)) * num24) * ((1.0 - (((num9 * num9) * (((5.0 + ((3.0 * num24) * num24)) + num23) - (((9.0 * num24) * num24) * num23))) / ((12.0 * num22) * num22))) + (((((num9 * num9) * num9) * num9) * ((61.0 + ((90.0 * num24) * num24)) + ((((45.0 * num24) * num24) * num24) * num24))) / ((((360.0 * num22) * num22) * num22) * num22)))) / ((2.0 * num22) * num22);
                    num27 = a - num27;
                    num2 = num27;
                    num /= num13;
                    num2 /= num13;
                }
                else
                {
                    double num30 = (num20 * (1.0 - num18)) * (((num14 - (num15 * Math.Cos(2.0 * a))) + (num16 * Math.Cos(4.0 * a))) - (num17 * Math.Cos(6.0 * a)));
                    double num4 = a;
                    int num31 = 1;
                    do
                    {
                        a = num4 - (num29 / num30);
                        if ((Math.Abs((double) (a - num4)) < num21) && (Math.Abs(num29) < num21))
                        {
                            break;
                        }
                        num28 = (num20 * (1.0 - num18)) * ((((num14 * a) - ((num15 * (Math.Sin(2.0 * a) - Math.Sin(0.0))) / 2.0)) + ((num16 * (Math.Sin(4.0 * a) - Math.Sin(0.0))) / 4.0)) - ((num17 * (Math.Sin(6.0 * a) - Math.Sin(0.0))) / 6.0));
                        num29 = num28 - num10;
                        num30 = (num20 * (1.0 - num18)) * (((num14 - (num15 * Math.Cos(2.0 * a))) + (num16 * Math.Cos(4.0 * a))) - (num17 * Math.Cos(6.0 * a)));
                        num4 = a;
                        num31++;
                    }
                    while (num31 <= 100);
                    num22 = num20 / Math.Sqrt(1.0 - ((num18 * Math.Sin(a)) * Math.Sin(a)));
                    num23 = (num19 * Math.Cos(a)) * Math.Cos(a);
                    num24 = Math.Tan(a);
                    num25 = Math.Abs(num9) / num22;
                    num26 = (num25 * ((1.0 - (((num25 * num25) * ((1.0 + ((2.0 * num24) * num24)) + num23)) / 6.0)) + (((((num25 * num25) * num25) * num25) * ((((5.0 + ((28.0 * num24) * num24)) + ((((24.0 * num24) * num24) * num24) * num24)) + (6.0 * num23)) + (((8.0 * num23) * num24) * num24))) / 120.0))) / Math.Cos(a);
                    if (num11 == 0)
                    {
                        num26 += num12;
                    }
                    else
                    {
                        num26 = num12 - num26;
                    }
                    num = num26;
                    num27 = ((((num9 * num9) * (1.0 + num23)) * num24) * ((1.0 - (((num9 * num9) * (((5.0 + ((3.0 * num24) * num24)) + num23) - (((9.0 * num24) * num24) * num23))) / ((12.0 * num22) * num22))) + (((((num9 * num9) * num9) * num9) * ((61.0 + ((90.0 * num24) * num24)) + ((((45.0 * num24) * num24) * num24) * num24))) / ((((360.0 * num22) * num22) * num22) * num22)))) / ((2.0 * num22) * num22);
                    num27 = a - num27;
                    num2 = num27;
                    num /= num13;
                    num2 /= num13;
                    int num32 = (int) num;
                    double num36 = (num - num32) * 60.0;
                    int num34 = (int) num36;
                    num36 = (num36 - num34) * 60.0;
                    float num37 = Convert.ToInt32(num36);
                    int num33 = (int) num2;
                    num36 = (num2 - num33) * 60.0;
                    int num35 = (int) num36;
                    num36 = (num36 - num35) * 60.0;
                    float num38 = Convert.ToInt32(num36);
                    outx = string.Concat(new object[] { num32, "度", num34, "分", num37, "秒" });
                    outy = string.Concat(new object[] { num33, "度", num35, "分", num38, "秒" });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void bj54tojingweidu2(double inx, double iny, ref string outx, ref string outy)
        {
            try
            {
                double num;
                double num2;
                int num11;
                double num12;
                double num22;
                double num23;
                double num24;
                double num25;
                double num26;
                double num27;
                double num5 = inx;
                double num6 = iny;
                int num7 = 2;
                int num8 = (int) (num5 / 1000000.0);
                double num9 = num5 - (num8 * 1000000.0);
                double num10 = num6;
                if (num9 > 500000.0)
                {
                    num9 -= 500000.0;
                    num11 = 0;
                }
                else
                {
                    num9 = 500000.0 - num9;
                    num11 = -1;
                }
                double num13 = 0.017453292519943295;
                double num14 = 1.0050517738936;
                double num15 = 0.0050623776450643;
                double num16 = 1.0624521037856E-05;
                double num17 = 2.080819597705E-08;
                double num18 = 0.0066934216229659;
                double num19 = 0.0067385254146834;
                double num20 = 6378245.0;
                double num21 = 1E-07;
                if (num7 == 1)
                {
                    num12 = ((num8 - 1) * 6) + 3;
                    num12 *= num13;
                }
                else
                {
                    num12 = num8 * 3;
                    num12 *= num13;
                }
                double a = Math.Abs(num10) / ((num14 * num20) * (1.0 - num18));
                double num28 = (num20 * (1.0 - num18)) * ((((num14 * a) - ((num15 * (Math.Sin(2.0 * a) - Math.Sin(0.0))) / 2.0)) + ((num16 * (Math.Sin(4.0 * a) - Math.Sin(0.0))) / 4.0)) - ((num17 * (Math.Sin(6.0 * a) - Math.Sin(0.0))) / 6.0));
                double num29 = num28 - num10;
                if (Math.Abs(num29) <= num21)
                {
                    num22 = num20 / Math.Sqrt(1.0 - ((num18 * Math.Sin(a)) * Math.Sin(a)));
                    num23 = (num19 * Math.Cos(a)) * Math.Cos(a);
                    num24 = Math.Tan(a);
                    num25 = Math.Abs(num9) / num22;
                    num26 = (num25 * ((1.0 - (((num25 * num25) * ((1.0 + ((2.0 * num24) * num24)) + num23)) / 6.0)) + (((((num25 * num25) * num25) * num25) * ((((5.0 + ((28.0 * num24) * num24)) + ((((24.0 * num24) * num24) * num24) * num24)) + (6.0 * num23)) + (((8.0 * num23) * num24) * num24))) / 120.0))) / Math.Cos(a);
                    if (num11 == 0)
                    {
                        num26 += num12;
                    }
                    else
                    {
                        num26 = num12 - num26;
                    }
                    num = num26;
                    num27 = ((((num9 * num9) * (1.0 + num23)) * num24) * ((1.0 - (((num9 * num9) * (((5.0 + ((3.0 * num24) * num24)) + num23) - (((9.0 * num24) * num24) * num23))) / ((12.0 * num22) * num22))) + (((((num9 * num9) * num9) * num9) * ((61.0 + ((90.0 * num24) * num24)) + ((((45.0 * num24) * num24) * num24) * num24))) / ((((360.0 * num22) * num22) * num22) * num22)))) / ((2.0 * num22) * num22);
                    num27 = a - num27;
                    num2 = num27;
                    num /= num13;
                    num2 /= num13;
                }
                else
                {
                    double num30 = (num20 * (1.0 - num18)) * (((num14 - (num15 * Math.Cos(2.0 * a))) + (num16 * Math.Cos(4.0 * a))) - (num17 * Math.Cos(6.0 * a)));
                    double num4 = a;
                    int num31 = 1;
                    do
                    {
                        a = num4 - (num29 / num30);
                        if ((Math.Abs((double) (a - num4)) < num21) && (Math.Abs(num29) < num21))
                        {
                            break;
                        }
                        num28 = (num20 * (1.0 - num18)) * ((((num14 * a) - ((num15 * (Math.Sin(2.0 * a) - Math.Sin(0.0))) / 2.0)) + ((num16 * (Math.Sin(4.0 * a) - Math.Sin(0.0))) / 4.0)) - ((num17 * (Math.Sin(6.0 * a) - Math.Sin(0.0))) / 6.0));
                        num29 = num28 - num10;
                        num30 = (num20 * (1.0 - num18)) * (((num14 - (num15 * Math.Cos(2.0 * a))) + (num16 * Math.Cos(4.0 * a))) - (num17 * Math.Cos(6.0 * a)));
                        num4 = a;
                        num31++;
                    }
                    while (num31 <= 100);
                    num22 = num20 / Math.Sqrt(1.0 - ((num18 * Math.Sin(a)) * Math.Sin(a)));
                    num23 = (num19 * Math.Cos(a)) * Math.Cos(a);
                    num24 = Math.Tan(a);
                    num25 = Math.Abs(num9) / num22;
                    num26 = (num25 * ((1.0 - (((num25 * num25) * ((1.0 + ((2.0 * num24) * num24)) + num23)) / 6.0)) + (((((num25 * num25) * num25) * num25) * ((((5.0 + ((28.0 * num24) * num24)) + ((((24.0 * num24) * num24) * num24) * num24)) + (6.0 * num23)) + (((8.0 * num23) * num24) * num24))) / 120.0))) / Math.Cos(a);
                    if (num11 == 0)
                    {
                        num26 += num12;
                    }
                    else
                    {
                        num26 = num12 - num26;
                    }
                    num = num26;
                    num27 = ((((num9 * num9) * (1.0 + num23)) * num24) * ((1.0 - (((num9 * num9) * (((5.0 + ((3.0 * num24) * num24)) + num23) - (((9.0 * num24) * num24) * num23))) / ((12.0 * num22) * num22))) + (((((num9 * num9) * num9) * num9) * ((61.0 + ((90.0 * num24) * num24)) + ((((45.0 * num24) * num24) * num24) * num24))) / ((((360.0 * num22) * num22) * num22) * num22)))) / ((2.0 * num22) * num22);
                    num27 = a - num27;
                    num2 = num27;
                    num /= num13;
                    num2 /= num13;
                    int num32 = (int) num;
                    double num36 = (num - num32) * 60.0;
                    int num34 = (int) num36;
                    num36 = (num36 - num34) * 60.0;
                    float num37 = Convert.ToInt32(num36);
                    int num33 = (int) num2;
                    num36 = (num2 - num33) * 60.0;
                    int num35 = (int) num36;
                    num36 = (num36 - num35) * 60.0;
                    float num38 = Convert.ToInt32(num36);
                    outx = string.Concat(new object[] { num32, "\x00b0", num34, "′", num37, "″" });
                    outy = string.Concat(new object[] { num33, "\x00b0", num35, "′", num38, "″" });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void bj54tojingweidu3(double inx, double iny, ref string outx, ref string outy)
        {
            try
            {
                double num;
                double num2;
                int num11;
                double num12;
                double num22;
                double num23;
                double num24;
                double num25;
                double num26;
                double num27;
                double num5 = inx;
                double num6 = iny;
                int num7 = 2;
                int num8 = (int) (num5 / 1000000.0);
                double num9 = num5 - (num8 * 1000000.0);
                double num10 = num6;
                if (num9 > 500000.0)
                {
                    num9 -= 500000.0;
                    num11 = 0;
                }
                else
                {
                    num9 = 500000.0 - num9;
                    num11 = -1;
                }
                double num13 = 0.017453292519943295;
                double num14 = 1.0050517738936;
                double num15 = 0.0050623776450643;
                double num16 = 1.0624521037856E-05;
                double num17 = 2.080819597705E-08;
                double num18 = 0.0066934216229659;
                double num19 = 0.0067385254146834;
                double num20 = 6378245.0;
                double num21 = 1E-07;
                if (num7 == 1)
                {
                    num12 = ((num8 - 1) * 6) + 3;
                    num12 *= num13;
                }
                else
                {
                    num12 = num8 * 3;
                    num12 *= num13;
                }
                double a = Math.Abs(num10) / ((num14 * num20) * (1.0 - num18));
                double num28 = (num20 * (1.0 - num18)) * ((((num14 * a) - ((num15 * (Math.Sin(2.0 * a) - Math.Sin(0.0))) / 2.0)) + ((num16 * (Math.Sin(4.0 * a) - Math.Sin(0.0))) / 4.0)) - ((num17 * (Math.Sin(6.0 * a) - Math.Sin(0.0))) / 6.0));
                double num29 = num28 - num10;
                if (Math.Abs(num29) <= num21)
                {
                    num22 = num20 / Math.Sqrt(1.0 - ((num18 * Math.Sin(a)) * Math.Sin(a)));
                    num23 = (num19 * Math.Cos(a)) * Math.Cos(a);
                    num24 = Math.Tan(a);
                    num25 = Math.Abs(num9) / num22;
                    num26 = (num25 * ((1.0 - (((num25 * num25) * ((1.0 + ((2.0 * num24) * num24)) + num23)) / 6.0)) + (((((num25 * num25) * num25) * num25) * ((((5.0 + ((28.0 * num24) * num24)) + ((((24.0 * num24) * num24) * num24) * num24)) + (6.0 * num23)) + (((8.0 * num23) * num24) * num24))) / 120.0))) / Math.Cos(a);
                    if (num11 == 0)
                    {
                        num26 += num12;
                    }
                    else
                    {
                        num26 = num12 - num26;
                    }
                    num = num26;
                    num27 = ((((num9 * num9) * (1.0 + num23)) * num24) * ((1.0 - (((num9 * num9) * (((5.0 + ((3.0 * num24) * num24)) + num23) - (((9.0 * num24) * num24) * num23))) / ((12.0 * num22) * num22))) + (((((num9 * num9) * num9) * num9) * ((61.0 + ((90.0 * num24) * num24)) + ((((45.0 * num24) * num24) * num24) * num24))) / ((((360.0 * num22) * num22) * num22) * num22)))) / ((2.0 * num22) * num22);
                    num27 = a - num27;
                    num2 = num27;
                    num /= num13;
                    num2 /= num13;
                }
                else
                {
                    double num30 = (num20 * (1.0 - num18)) * (((num14 - (num15 * Math.Cos(2.0 * a))) + (num16 * Math.Cos(4.0 * a))) - (num17 * Math.Cos(6.0 * a)));
                    double num4 = a;
                    int num31 = 1;
                    do
                    {
                        a = num4 - (num29 / num30);
                        if ((Math.Abs((double) (a - num4)) < num21) && (Math.Abs(num29) < num21))
                        {
                            break;
                        }
                        num28 = (num20 * (1.0 - num18)) * ((((num14 * a) - ((num15 * (Math.Sin(2.0 * a) - Math.Sin(0.0))) / 2.0)) + ((num16 * (Math.Sin(4.0 * a) - Math.Sin(0.0))) / 4.0)) - ((num17 * (Math.Sin(6.0 * a) - Math.Sin(0.0))) / 6.0));
                        num29 = num28 - num10;
                        num30 = (num20 * (1.0 - num18)) * (((num14 - (num15 * Math.Cos(2.0 * a))) + (num16 * Math.Cos(4.0 * a))) - (num17 * Math.Cos(6.0 * a)));
                        num4 = a;
                        num31++;
                    }
                    while (num31 <= 100);
                    num22 = num20 / Math.Sqrt(1.0 - ((num18 * Math.Sin(a)) * Math.Sin(a)));
                    num23 = (num19 * Math.Cos(a)) * Math.Cos(a);
                    num24 = Math.Tan(a);
                    num25 = Math.Abs(num9) / num22;
                    num26 = (num25 * ((1.0 - (((num25 * num25) * ((1.0 + ((2.0 * num24) * num24)) + num23)) / 6.0)) + (((((num25 * num25) * num25) * num25) * ((((5.0 + ((28.0 * num24) * num24)) + ((((24.0 * num24) * num24) * num24) * num24)) + (6.0 * num23)) + (((8.0 * num23) * num24) * num24))) / 120.0))) / Math.Cos(a);
                    if (num11 == 0)
                    {
                        num26 += num12;
                    }
                    else
                    {
                        num26 = num12 - num26;
                    }
                    num = num26;
                    num27 = ((((num9 * num9) * (1.0 + num23)) * num24) * ((1.0 - (((num9 * num9) * (((5.0 + ((3.0 * num24) * num24)) + num23) - (((9.0 * num24) * num24) * num23))) / ((12.0 * num22) * num22))) + (((((num9 * num9) * num9) * num9) * ((61.0 + ((90.0 * num24) * num24)) + ((((45.0 * num24) * num24) * num24) * num24))) / ((((360.0 * num22) * num22) * num22) * num22)))) / ((2.0 * num22) * num22);
                    num27 = a - num27;
                    num2 = num27;
                    num /= num13;
                    num2 /= num13;
                    outx = num.ToString();
                    outy = num2.ToString();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void bj54tojingweiduAo(IActiveView pActiveView, double inx, double iny, ref string outx, ref string outy)
        {
            try
            {
                IMap focusMap = pActiveView.FocusMap;
                SpatialReferenceEnvironment environment = new SpatialReferenceEnvironmentClass();
                ISpatialReference newReferenceSystem = environment.CreateGeographicCoordinateSystem(0x1076);
                IPoint point = new PointClass {
                    X = inx,
                    Y = iny
                };
                IGeometry geometry = point;
                geometry.SpatialReference = focusMap.SpatialReference;
                geometry.Project(newReferenceSystem);
                if (geometry.IsEmpty)
                {
                    outx = "超出范围";
                    outy = "超出范围";
                }
                else
                {
                    double x = point.X;
                    double y = point.Y;
                    int num3 = (int) x;
                    double num7 = (x - num3) * 60.0;
                    int num5 = (int) num7;
                    num7 = (num7 - num5) * 60.0;
                    float num8 = Convert.ToInt32(num7);
                    int num4 = (int) y;
                    num7 = (y - num4) * 60.0;
                    int num6 = (int) num7;
                    num7 = (num7 - num6) * 60.0;
                    float num9 = Convert.ToInt32(num7);
                    outx = string.Concat(new object[] { num3, "度", num5, "分", num8, "秒" });
                    outy = string.Concat(new object[] { num4, "度", num6, "分", num9, "秒" });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void bj54tojingweiduAo2(IActiveView pActiveView, double inx, double iny, ref string outx, ref string outy)
        {
            try
            {
                IMap focusMap = pActiveView.FocusMap;
                SpatialReferenceEnvironment environment = new SpatialReferenceEnvironmentClass();
                ISpatialReference newReferenceSystem = environment.CreateGeographicCoordinateSystem(0x1076);
                IPoint point = new PointClass {
                    X = inx,
                    Y = iny
                };
                IGeometry geometry = point;
                geometry.SpatialReference = focusMap.SpatialReference;
                geometry.Project(newReferenceSystem);
                if (geometry.IsEmpty)
                {
                    outx = "超出范围";
                    outy = "超出范围";
                }
                else
                {
                    double x = point.X;
                    double y = point.Y;
                    int num3 = (int) x;
                    double num7 = (x - num3) * 60.0;
                    int num5 = (int) num7;
                    num7 = (num7 - num5) * 60.0;
                    double num8 = Math.Round(num7, 2);
                    int num4 = (int) y;
                    num7 = (y - num4) * 60.0;
                    int num6 = (int) num7;
                    num7 = (num7 - num6) * 60.0;
                    double num9 = Math.Round(num7, 2);
                    outx = string.Concat(new object[] { num3, "\x00b0", num5, "′", num8, "″" });
                    outy = string.Concat(new object[] { num4, "\x00b0", num6, "′", num9, "″" });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public double ConvertESRIUnits(double dValue, IMap pMap, esriUnits eOutUnits)
        {
            try
            {
                if (pMap == null)
                {
                    return 0.0;
                }
                if (pMap.MapUnits == eOutUnits)
                {
                    return dValue;
                }
                return this.ConvertESRIUnits(dValue, pMap.MapUnits, eOutUnits);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.UnitFun", "ConvertESRIUnits", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1.0;
            }
        }

        public double ConvertESRIUnits(double dValue, esriUnits eInUnits, esriUnits eOutUnits)
        {
            try
            {
                if (this.mESRIConvertUnits == null)
                {
                    this.mESRIConvertUnits = new UnitConverterClass();
                }
                return this.mESRIConvertUnits.ConvertUnits(dValue, eInUnits, eOutUnits);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.UnitFun", "ConvertESRIUnits", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1.0;
            }
        }

        public double ConvertMapToPixelsUnits(IActiveView pActiveView, double dMapUnits, bool bWidthFlag)
        {
            try
            {
                if (pActiveView == null)
                {
                    return 0.0;
                }
                if (dMapUnits == 0.0)
                {
                    return 0.0;
                }
                IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
                IEnvelope visibleBounds = displayTransformation.VisibleBounds;
                tagRECT deviceFrame = displayTransformation.get_DeviceFrame();
                double width = 0.0;
                long num2 = 0L;
                if (bWidthFlag)
                {
                    width = visibleBounds.Width;
                    num2 = deviceFrame.right - deviceFrame.left;
                }
                else
                {
                    width = visibleBounds.Height;
                    num2 = deviceFrame.bottom - deviceFrame.top;
                }
                if (width == 0.0)
                {
                    return 0.0;
                }
                if (num2 == 0L)
                {
                    return 0.0;
                }
                double num3 = 0.0;
                num3 = width / ((double) num2);
                return (dMapUnits / num3);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.UnitFun", "ConvertMapToPixelsUnits", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1.0;
            }
        }

        public double ConvertPixelsToMapUnits(IActiveView pActiveView, double dPixelUnits, bool bWidthFlag)
        {
            try
            {
                if (pActiveView == null)
                {
                    return 0.0;
                }
                if (dPixelUnits == 0.0)
                {
                    return 0.0;
                }
                IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
                IEnvelope visibleBounds = displayTransformation.VisibleBounds;
                tagRECT deviceFrame = displayTransformation.get_DeviceFrame();
                double width = 0.0;
                long num2 = 0L;
                if (bWidthFlag)
                {
                    width = visibleBounds.Width;
                    num2 = deviceFrame.right - deviceFrame.left;
                }
                else
                {
                    width = visibleBounds.Height;
                    num2 = deviceFrame.bottom - deviceFrame.top;
                }
                if (width == 0.0)
                {
                    return 0.0;
                }
                if (num2 == 0L)
                {
                    return 0.0;
                }
                double num3 = 0.0;
                num3 = width / ((double) num2);
                return (dPixelUnits * num3);
            }
            catch (Exception exception)
            {
                this.mErrOpt.ErrorOperate(this.mSubSysName, "FunFactory.UnitFun", "ConvertMapToPixelsUnits", exception.GetHashCode().ToString(), exception.Source, exception.Message, "", "", "");
                return -1.0;
            }
        }

        public IGeometry ConvertPoject(IGeometry pGeometry, ISpatialReference pSpatialReference)
        {
            try
            {
                if (pGeometry.SpatialReference != pSpatialReference)
                {
                    pGeometry.Project(pSpatialReference);
                    pGeometry.SpatialReference = pSpatialReference;
                }
                return pGeometry;
            }
            catch (Exception)
            {
                return pGeometry;
            }
        }

        public void degreeminutesecond(double jwd_jd, double jwd_wd, ref string outx, ref string outy)
        {
            try
            {
                int num = (int) jwd_jd;
                double num5 = (jwd_jd - num) * 60.0;
                int num3 = (int) num5;
                num5 = (num5 - num3) * 60.0;
                float num6 = Convert.ToInt32(num5);
                int num2 = (int) jwd_wd;
                num5 = (jwd_wd - num2) * 60.0;
                int num4 = (int) num5;
                num5 = (num5 - num4) * 60.0;
                float num7 = Convert.ToInt32(num5);
                outx = string.Concat(new object[] { num, "\x00b0", num3, "′", num6, "″" });
                outy = string.Concat(new object[] { num2, "\x00b0", num4, "′", num7, "″" });
            }
            catch (Exception)
            {
            }
        }
    }
}

