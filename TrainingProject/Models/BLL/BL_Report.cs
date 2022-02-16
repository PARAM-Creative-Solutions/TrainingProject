using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Web.UI.WebControls;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.Constants;
using System.Collections.Generic;
using TrainingProject.ViewModels.Report;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using TrainingProject.Security;
using System.Web;
using System.Linq;
using TrainingProjectDataLayer.Reports;

namespace TrainingProject
{
    /// <summary>
    /// ReportGenerator
    /// </summary>
    public class ReportGenerator
	{
		private float multipageRatio = 1.5F;
		private CultureInfo ci = new CultureInfo("en-US");
		private DataTable dt = null;
        private DataTable detailDt;
		private string dsName = "";
        private string detaildsName = "";
        private string nsRd = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";
		private string ns = "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dsName"></param>
		public ReportGenerator(DataTable dt, string dsName)
		{
			this.dt = dt;
			this.dsName = dsName;
		}
        /// <summary>
        /// Overloaded for Detailed Table report 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dsName"></param>
        /// <param name="DetailedDt"></param>
        /// <param name="detailDsName"></param>
        public ReportGenerator(DataTable dt, string dsName , DataTable DetailedDt, string detailDsName)
        {
            this.dt = dt;
            this.dsName = dsName;
            this.detailDt = DetailedDt;
            this.detaildsName = detailDsName;
        }
/// <summary>
/// Overloaded for sigle table report
/// </summary>
/// <param name="Header"></param>
/// <param name="reportDesignType"></param>
/// <returns></returns>
        public Stream GeneraReport( string Header , SystemEnums.ReportDesignType  reportDesignType)
		{
			string xml;
			StringBuilder sb = new StringBuilder();
			#region Settings
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CheckCharacters = true;
			settings.CloseOutput = true;
			settings.Encoding = Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.NewLineChars = "\r\n";
			settings.NewLineHandling = NewLineHandling.Replace;
			settings.NewLineOnAttributes = false;
			settings.OmitXmlDeclaration = false;
			#endregion
			XmlWriter writer = XmlWriter.Create(sb, settings);
			writer.WriteStartDocument();
			{
				writer.WriteStartElement("Report", ns);
				writer.WriteAttributeString("xmlns", "rd", "", nsRd);
				{
					AddDataSource(writer, dsName);
					float htb = 0.63492F, maxWidth = 4.0F;
					RectangleF dimensioni = new RectangleF(0.4201668F, 2.0287F, 13.25397F, 2.98941F);
					Padding pad = new Padding(2, 2, 2, 2);
					SizeF size = new SizeF(21F, 29.5F);
					Padding margin = new Padding(0.5F, 0.5F, 0.5F, 0.5F);
					GenerateSettingsHeader(writer, size, size, margin);

					AddDataSet(writer, dt, dsName);
					writer.WriteStartElement("Body");
					{
						writer.WriteElementString("ColumnSpacing", "1cm");
						writer.WriteElementString("Height", "5cm");
						writer.WriteStartElement("ReportItems");
						{
                            
                           

                          float TableWidth =  GeneraTabella(writer, dt, dsName, dimensioni, pad, pad, htb, maxWidth);


                            #region Report Header
                            PointF startLocation = new PointF(TableWidth != 0.0 ? TableWidth / 2 : 9.37301F, 0.68783F);
                            SizeF headerSize = new SizeF(GetStringWidth(Header), 0.635F);
                            RectangleF ReportHeader = new RectangleF(startLocation, headerSize);


                            GeneraTextBox(writer, "Heading", ReportHeader, pad, new CellColors(System.Drawing.Color.White, System.Drawing.Color.Black, "None"), Header, SezioneTabella.Details);

                            #endregion


                        }
                        writer.WriteEndElement();
					} 
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
			xml = sb.ToString().Replace("utf-16", "utf-8");
            MemoryStream ret = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            //FileStream s = new FileStream(@"D:\ABC.txt", FileMode.Append, System.IO.FileAccess.Write);
            //ret.CopyTo(s);

            return ret;
		}

        /// <summary>
        /// Overloaded to create Detailed Table report
        /// </summary>
        /// <param name="Header"></param>
        /// <returns></returns>
        public Stream GeneraReport(string Header)
        {
            string xml;
            StringBuilder sb = new StringBuilder();
            #region Settings
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = true;
            settings.CloseOutput = true;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = false;
            #endregion
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            {
                writer.WriteStartElement("Report", ns);
                writer.WriteAttributeString("xmlns", "rd", "", nsRd);
                {
                    AddDataSource(writer, dsName, detaildsName);
                    
                    float htb = 0.63492F, maxWidth = 4.0F;
                    float startX = 0.4201668F;
                    float startY = 2.0287F;
                    
                    Padding pad = new Padding(2, 2, 2, 2);
                    SizeF size = new SizeF(21F, 29.5F);
                    Padding margin = new Padding(0.5F, 0.5F, 0.5F, 0.5F);
                    GenerateSettingsHeader(writer, size, size, margin);

                    AddDataSet(writer, dt, dsName);
                   // AddDataSet(writer, detailDt, "dataset2");
                    writer.WriteStartElement("Body");
                    {
                        writer.WriteElementString("ColumnSpacing", "1cm");
                        writer.WriteElementString("Height", "5cm");
                        writer.WriteStartElement("ReportItems");
                        {
                            #region ADD Report DETAILS


                            #region Claculate Report Detail Height
                            float TextBoxHeight = 0.68783F;
                            float TextBoxMarginY = 0.003F;
                            float TextBoxMarginX = 0.003F;
                            float TotaldeatilHeight = 0;
                            
                            CellColors DetailTextbox = new CellColors(System.Drawing.Color.White, System.Drawing.Color.Black, "None");
                           #region Get max Text Box length
                            float LeftTexboxMaxWidth = 0;
                            for (int i = 0; i < detailDt.Columns.Count; i++)
                            {
                                float current = GetStringWidth(detailDt.Columns[i].Caption);
                                if (LeftTexboxMaxWidth < current)
                                {
                                    LeftTexboxMaxWidth = current;
                                }
                                

                            }
                            #endregion
                            float nextRowTextBoxY = 0;
                            for (int i = 0; i < detailDt.Columns.Count; i++)
                            {

                                #region LeftTextBox
                                SizeF lefttextboxsize = new SizeF(LeftTexboxMaxWidth, TextBoxHeight);
                                PointF LeftTextBox = new PointF(startX, startY+ nextRowTextBoxY);
                                RectangleF leftrect = new RectangleF(LeftTextBox, lefttextboxsize);
                                GeneraTextBox(writer, "Label" + i.ToString(), leftrect, pad, DetailTextbox, detailDt.Columns[i].Caption+"::", SezioneTabella.Header);
                               
                                #endregion

                                #region RightTextBox
                                string Value = detailDt.Rows[0][i].ToString();
                                SizeF Righttextboxsize = new SizeF(GetStringWidth(Value), TextBoxHeight);
                                PointF RightTextBox = new PointF(startX+ LeftTexboxMaxWidth+ TextBoxMarginX, startY + nextRowTextBoxY);
                                RectangleF Rightrect = new RectangleF(RightTextBox, Righttextboxsize);
                                GeneraTextBox(writer, "TextLabel" + i.ToString(), Rightrect, pad, DetailTextbox, Value, SezioneTabella.Details);
                                #endregion

                                TotaldeatilHeight += (TextBoxHeight+ TextBoxMarginY);
                                nextRowTextBoxY += TextBoxHeight + TextBoxMarginY;
                            }
                            #endregion



                            #endregion

                            RectangleF dimensioni = new RectangleF(startX, startY+ TotaldeatilHeight, 13.25397F, 2.98941F);
                            float TableWidth = GeneraTabella(writer, dt, dsName, dimensioni, pad, pad, htb, maxWidth);


                            #region Report Header
                            PointF startLocation = new PointF(TableWidth != 0.0 ? TableWidth / 2 : 9.37301F, 0.68783F);
                            SizeF headerSize = new SizeF(GetStringWidth(Header), 0.635F);
                            RectangleF ReportHeader = new RectangleF(startLocation, headerSize);


                            GeneraTextBox(writer, "Heading", ReportHeader, pad, new CellColors(System.Drawing.Color.White, System.Drawing.Color.Black, "None"), Header, SezioneTabella.Details);

                            #endregion

                           


                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            xml = sb.ToString().Replace("utf-16", "utf-8");
            Stream ret = new MemoryStream(Encoding.UTF8.GetBytes(xml));
           
            return ret;
        }

        #region Metodi Privati
        private SizeF GetDynamicSize(string s)
		{
			Font f = new Font(FontFamily.GenericSansSerif, 10);
			Bitmap bmp = new Bitmap(1, 1);
			Graphics g = Graphics.FromImage(bmp);
			g.PageUnit = GraphicsUnit.Millimeter;
			SizeF ret = SizeF.Empty;
			ret = g.MeasureString(s, f);
			g.Dispose();
			return ret;
		}

        private float  GetStringWidth(string s)
        {
            return GetDynamicSize(s).Width /8;
        }

		private void GeneraSezioneTabella(SezioneTabella sezione, XmlWriter writer, DataTable dt, Padding padding, float height)
		{
			string nomeSezione = "", templateValore = "", valore = "";
			CellColors colors = null;
			switch (sezione)
			{
				case SezioneTabella.Header:
					{
						nomeSezione = "Header";
						templateValore = "{0}";
						colors = new CellColors(System.Drawing.Color.Black, System.Drawing.Color.White);
						break;
					}
				case SezioneTabella.Details:
					{
						nomeSezione = "Details";
						templateValore = "=Fields!{0}.Value";
						break;
					}
				case SezioneTabella.Footer:
					{
						nomeSezione = "Footer";
						templateValore = "{0}";
						break;
					}
			}
			writer.WriteStartElement(nomeSezione);
			{
				if (sezione == SezioneTabella.Header)
					writer.WriteElementString("RepeatOnNewPage", "true");
				writer.WriteStartElement("TableRows");
				{
					writer.WriteStartElement("TableRow");
					{
						writer.WriteElementString("Height", height.ToString(ci) + "cm");
						writer.WriteStartElement("TableCells");
						{
							for (int i = 0; i < dt.Columns.Count; i++)
							{
								writer.WriteStartElement("TableCell");
								{
									writer.WriteStartElement("ReportItems");
                                    {
                                        string name = string.Empty;
                                        if (sezione == SezioneTabella.Header || sezione == SezioneTabella.Footer)
                                            name = dt.Columns[i].Caption ;
                                        else
                                            name = dt.Columns[i].ColumnName;

                                        valore = String.Format(templateValore, name);
										GeneraTextBox(writer, "textbox" + nomeSezione + i, RectangleF.Empty, padding, colors, valore, sezione);
									}
									writer.WriteEndElement();
								}
								writer.WriteEndElement();
							}
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		private float GeneraTabella(XmlWriter writer, DataTable dt, string dsName, RectangleF dimensioniTabella, Padding paddingTextBox, Padding paddingHeader, float heightTextBox, float MaxWidth)
		{
            float TableWidth = 0;

			writer.WriteStartElement("Table");
            
            writer.WriteAttributeString("Name", "tabella" + dsName);
			{
				writer.WriteStartElement("Style");
				{
					writer.WriteStartElement("BorderStyle");
					{
						writer.WriteElementString("Default", "Solid");
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();

				writer.WriteElementString("Top", dimensioniTabella.Top.ToString(ci) + "cm");
				writer.WriteElementString("Left", dimensioniTabella.Left.ToString(ci) + "cm");
				writer.WriteElementString("Width", dimensioniTabella.Width.ToString(ci) + "cm");
				writer.WriteElementString("Height", dimensioniTabella.Height.ToString(ci) + "cm");

				writer.WriteStartElement("TableColumns");
				{
					for (int i = 0; i < dt.Columns.Count; i++)
					{
						writer.WriteStartElement("TableColumn");
						{
							DataColumn dc = dt.Columns[i];
							float sizeWidthComputed = 0.0F;
							float RowMaxLength = GetDynamicSize(dt.Rows[0][i].ToString()).Width / 10;
							float HeaderMaxLength = (GetDynamicSize(dc.Caption).Width / 10) + 0.2F;
							foreach (DataRow row in dt.Rows)
							{
								float rowSizeWidth = GetDynamicSize(row[i].ToString()).Width / 10;
								if (rowSizeWidth > RowMaxLength)
									RowMaxLength = rowSizeWidth;
							}

							if (RowMaxLength > HeaderMaxLength)
								if (RowMaxLength > MaxWidth)
									sizeWidthComputed = MaxWidth;
								else
									sizeWidthComputed = RowMaxLength;
							else
								sizeWidthComputed = HeaderMaxLength;
                            //ADDED BY ANAGHA
                            TableWidth += sizeWidthComputed;

                            writer.WriteElementString("Width", (sizeWidthComputed).ToString(ci) + "cm");
						}
						writer.WriteEndElement();
					}
				}
				writer.WriteEndElement();

				GeneraSezioneTabella(SezioneTabella.Header, writer, dt, paddingHeader, heightTextBox);
				GeneraSezioneTabella(SezioneTabella.Details, writer, dt, paddingTextBox, heightTextBox);
			}
            if (detailDt != null)
            {

                writer.WriteElementString("DataSetName", dsName);
            }
            writer.WriteEndElement();

            return TableWidth;
		}

		private void AddDataSet(XmlWriter writer, DataTable dt, string dsName)
		{
			writer.WriteStartElement("DataSets");
			{
				writer.WriteStartElement("DataSet");
				writer.WriteAttributeString("Name", dsName);
				{
					writer.WriteStartElement("Fields");
					{
						for (int i = 0; i < dt.Columns.Count; i++)
						{
							writer.WriteStartElement("Field");
							writer.WriteAttributeString("Name", dt.Columns[i].ColumnName);
							{
								writer.WriteElementString("DataField", dt.Columns[i].ColumnName);
								writer.WriteElementString("rd", "TypeName", nsRd, dt.Columns[i].DataType.ToString());
							}
							writer.WriteEndElement();
						}
					}
					writer.WriteEndElement();
                    
					writer.WriteStartElement("Query");
					{
						writer.WriteElementString("DataSourceName", dsName);
						writer.WriteElementString("CommandText", "");
						writer.WriteElementString("rd", "DataSourceName", nsRd, "true");
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();

                if (detailDt!=null)
                {
                    writer.WriteStartElement("DataSet");
                    writer.WriteAttributeString("Name", detaildsName);
                    {
                        writer.WriteStartElement("Fields");
                        {
                            for (int i = 0; i < detailDt.Columns.Count; i++)
                            {
                                writer.WriteStartElement("Field");
                                writer.WriteAttributeString("Name", detailDt.Columns[i].ColumnName);
                                {
                                    writer.WriteElementString("DataField", detailDt.Columns[i].ColumnName);
                                    writer.WriteElementString("rd", "TypeName", nsRd, detailDt.Columns[i].DataType.ToString());
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("Query");
                        {
                            writer.WriteElementString("DataSourceName", detaildsName);
                            writer.WriteElementString("CommandText", "");
                            writer.WriteElementString("rd", "DataSourceName", nsRd, "true");
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }

			}
			writer.WriteEndElement();
		}

        private void AddDataSource(XmlWriter writer, string dsName, string detaildatasetname = "")
		{
			writer.WriteStartElement("DataSources");
			{
				writer.WriteStartElement("DataSource");
				{
					writer.WriteAttributeString("Name", dsName);
					writer.WriteElementString("DataSourceReference", dsName);
				}
				writer.WriteEndElement();
                if (detaildatasetname!="")
                {
                    writer.WriteStartElement("DataSource");
                    {
                        writer.WriteAttributeString("Name", detaildatasetname);
                        writer.WriteElementString("DataSourceReference", detaildatasetname);
                    }
                    writer.WriteEndElement();
                }
			}
			writer.WriteEndElement();
		}

		private void GeneraTextBox(XmlWriter writer, string textboxName, RectangleF dimensioni, Padding padding, CellColors colors, string value, SezioneTabella sezione)
		{
			writer.WriteStartElement("Textbox");
			writer.WriteAttributeString("Name", textboxName);
			{
				writer.WriteElementString("rd", "DefaultName", nsRd, textboxName);
				if (dimensioni != RectangleF.Empty)
				{
					writer.WriteElementString("Top", dimensioni.Top.ToString(ci) + "cm");
					writer.WriteElementString("Left", dimensioni.Left.ToString(ci) + "cm");
					writer.WriteElementString("Width", dimensioni.Width.ToString(ci) + "cm");
					writer.WriteElementString("Height", dimensioni.Height.ToString(ci) + "cm");
				}
				writer.WriteElementString("CanGrow", "true");
               
                

				writer.WriteElementString("Value", value);
				if (padding != null)
				{
					writer.WriteStartElement("Style");
					{
						writer.WriteStartElement("BorderStyle");
                        {
                            if (colors!=null)
                            {
                                writer.WriteElementString("Default", colors.Border);
                            }
                            else
                                writer.WriteElementString("Default", "Solid");

                        }
						writer.WriteEndElement();

						if (colors != null)
						{
							writer.WriteElementString("System.Drawing.Color", colors.ForegroundColor.Name);
							writer.WriteElementString("BackgroundColor", colors.BackgroundColor.Name);
						}

						writer.WriteElementString("PaddingLeft", padding.Left.ToString(ci) + "pt");
						writer.WriteElementString("PaddingRight", padding.Right.ToString(ci) + "pt");
						writer.WriteElementString("PaddingTop", padding.Top.ToString(ci) + "pt");
						writer.WriteElementString("PaddingBottom", padding.Bottom.ToString(ci) + "pt");
					}
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
		}

		private void GenerateSettingsHeader(XmlWriter writer, SizeF InteractiveSize, SizeF PageSize, Padding margin)
		{
			writer.WriteElementString("Language", "it-IT");
			writer.WriteElementString("rd", "DrawGrid", nsRd, "true");
			writer.WriteElementString("rd", "gridspacing", nsRd, "0.25cm");
			writer.WriteElementString("rd", "snaptogrid", nsRd, "true");
			writer.WriteElementString("InteractiveHeight", InteractiveSize.Height.ToString(ci) + "cm");
			writer.WriteElementString("InteractiveWidth", InteractiveSize.Width.ToString(ci) + "cm");
			writer.WriteElementString("RightMargin", margin.Right.ToString(ci) + "cm");
			writer.WriteElementString("LeftMargin", margin.Left.ToString(ci) + "cm");
			writer.WriteElementString("BottomMargin", margin.Bottom.ToString(ci) + "cm");
			writer.WriteElementString("TopMargin", margin.Top.ToString(ci) + "cm");
			writer.WriteElementString("PageHeight", PageSize.Height.ToString(ci) + "cm");
			writer.WriteElementString("PageWidth", PageSize.Width.ToString(ci) + "cm");
			writer.WriteElementString("Width", PageSize.Width.ToString(ci) + "cm");
		}
		#endregion
	}

    #region Oggetti Custom
    /// <summary>
    /// SezioneTabella
    /// </summary>
    public enum SezioneTabella
	{
        /// <summary>
        /// Header
        /// </summary>
		Header,
        /// <summary>
        /// Details
        /// </summary>
		Details,
        /// <summary>
        /// Footer
        /// </summary>
		Footer
    }

    /// <summary>
    /// CellColors
    /// </summary>
	public class CellColors
	{
        /// <summary>
        /// CellColors
        /// </summary>
        /// <param name="bg"></param>
        /// <param name="fore"></param>
        /// <param name="border"></param>
		public CellColors(System.Drawing.Color bg, System.Drawing.Color fore,string border= "Solid")
		{
			this.bg = bg;
			this.fore = fore;
            this.border = border;

        }
		private System.Drawing.Color bg = System.Drawing.Color.Empty;
		private System.Drawing.Color fore = System.Drawing.Color.Empty;
        private string border = string.Empty;

        /// <summary>
        /// BackgroundColor
        /// </summary>
		public System.Drawing.Color BackgroundColor { get { return bg; } }
        /// <summary>
        /// ForegroundColor
        /// </summary>
		public System.Drawing.Color ForegroundColor { get { return fore; } }

        /// <summary>
        /// Border
        /// </summary>
        public string  Border { get { return border; } }

    }

    /// <summary>
    /// Padding
    /// </summary>
	public class Padding
	{
        /// <summary>
        /// Padding
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="Left"></param>
        /// <param name="Bottom"></param>
        /// <param name="Right"></param>
		public Padding(float Top, float Left, float Bottom, float Right)
		{
			TopLeft = new PointF(Left, Top);
			BottomRight = new PointF(Right, Bottom);
		}

		private PointF TopLeft { get; set; }
		private PointF BottomRight { get; set; }

        /// <summary>
        /// Top
        /// </summary>
		public float Top { get { return TopLeft.Y; } }

        /// <summary>
        /// Left
        /// </summary>
		public float Left { get { return TopLeft.X; } }

        /// <summary>
        /// Bottom
        /// </summary>
		public float Bottom { get { return BottomRight.Y; } }
        /// <summary>
        /// Right
        /// </summary>
		public float Right { get { return BottomRight.X; } }
	}
    #endregion

    /// <summary>
    /// BL_Report
    /// </summary>
    public class BL_Report
    {
        #region Properties

        /// <summary>
        /// Unit of work
        /// </summary>
        UnitofWork uow { get; set; }

        /// <summary>
        /// prop for result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// property for message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// prop for ReportsViewModel
        /// </summary>
        public ReportsViewModel report { get; set; }

        /// <summary>
        /// prop for ReportsViewModel
        /// </summary>
        public string reporttitle { get; set; }

        CustomPrincipal CurrentUser { get { return HttpContext.Current.User as CustomPrincipal; } }


        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="puow">Unit of work</param>
        public BL_Report(UnitofWork puow)
        {
            uow = puow;

        }
        #endregion
        #region methods
        void Supplierwisecallreport()
        {
             
        }
        #endregion
    }


}
