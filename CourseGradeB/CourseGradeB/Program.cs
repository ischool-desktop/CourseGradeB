using System.Collections.Generic;
using System.Windows.Forms;
using FISCA;
using FISCA.Presentation;
using Framework.Security;
using JHSchool.Affair;
using System;
using DataRationality;
using K12.Presentation;
using JHSchool;
using Framework;
using FISCA.Data;
using System.Data;
using CourseGradeB.CourseExtendControls;
using CourseGradeB.EduAdminExtendControls;
using CourseGradeB.StudentExtendControls;
using FISCA.UDT;
using CourseGradeB.StuAdminExtendControls;
using CourseGradeB.EduAdminExtendControls.Ribbon;


namespace CourseGradeB
{
    public class Program
    {
        [MainMethod("")]
        public static void Main()
        {
            // 課程加入教師檢視
            Course.Instance.AddView(new TeacherCategoryView());

            //Sync UDT Table
            AccessHelper a = new AccessHelper();
            a.Select<ConductRecord>("uid is null");
            a.Select<CourseExtendRecord>("uid is null");
            a.Select<ConductSetting>("uid is null");
            a.Select<SubjectRecord>("uid is null");
            a.Select<GpaRef>("uid is null");

            #region 權限註冊
            // 學生
            Catalog ribbon = RoleAclSource.Instance["學生"]["功能按鈕"];
            ribbon.Add(new RibbonFeature("JHSchool.Student.Ribbon0169", "匯出學期歷程"));
            ribbon.Add(new RibbonFeature("JHSchool.Student.Ribbon0170", "匯入學期歷程"));
            ribbon.Add(new RibbonFeature("JHSchool.Student.SubjectScoreCalculate", "計算科目成績"));
            ribbon = RoleAclSource.Instance["學生"]["資料項目"];
            ribbon.Add(new DetailItemFeature("JHSchool.Student.Detail.SemsScore", "學期成績"));

            //班級
            ribbon = RoleAclSource.Instance["班級"]["功能按鈕"];
            ribbon.Add(new RibbonFeature("JHSchool.Class.Ribbon0070", "班級開課"));
            ribbon.Add(new RibbonFeature("JHSchool.Class.Ribbon0070.HrtConductInputForm", "指標輸入"));

            // 課程
            ribbon = RoleAclSource.Instance["課程"]["功能按鈕"];
            ribbon.Add(new RibbonFeature("JHSchool.Course.Ribbon0031", "匯出課程修課學生"));
            ribbon.Add(new RibbonFeature("JHSchool.Course.Ribbon0021", "匯入課程修課學生"));
            ribbon.Add(new RibbonFeature("JHSchool.Course.Ribbon0070.CourseScoreInputForm", "成績輸入"));
            ribbon.Add(new RibbonFeature("JHSchool.Course.Ribbon0070.SubjectConductInputForm", "指標輸入"));
            ribbon.Add(new RibbonFeature("JHSchool.Course.Ribbon0070.CourseGradeEditor", "批次修改開課年級"));
            ribbon = RoleAclSource.Instance["課程"]["資料項目"];
            ribbon.Add(new DetailItemFeature("JHSchool.Course.Detail.BasicInfo", "基本資料"));
            ribbon.Add(new DetailItemFeature("JHSchool.Course.Detail.AttendStudent", "修課學生"));

            //教務作業
            ribbon = RoleAclSource.Instance["教務作業"];
            //ribbon.Add(new RibbonFeature("JHSchool.EduAdmin.Ribbon0000", "評量名稱管理"));
            ribbon.Add(new RibbonFeature("JHSchool.EduAdmin.Ribbon.SubjectManager", "科目資料管理"));
            ribbon.Add(new RibbonFeature("CourseGradeB.EduAdminExtendControls.Ribbon.SetHoursOpeningForm", "開放時間管理"));
            ribbon.Add(new RibbonFeature("CourseGradeB.EduAdminExtendControls.Ribbon.SubjectScoreCalculateByGradeyear", "批次計算科目成績"));
            ribbon.Add(new RibbonFeature("CourseGradeB.EduAdminExtendControls.Ribbon.GpaRefForm", "歷屆GPA統計"));
            ribbon.Add(new RibbonFeature("CourseGradeB.EduAdminExtendControls.Ribbon.SemsHistoryMaker", "產生學期歷程"));
            ribbon.Add(new RibbonFeature("CourseGradeB.EduAdminExtendControls.Ribbon.CourseScoreStatusForm", "評量輸入狀況檢視"));
            ribbon.Add(new RibbonFeature("CourseGradeB.EduAdminExtendControls.Ribbon.SubjectConductStatusForm", "Conduct輸入狀況檢視(授課老師)"));
            ribbon.Add(new RibbonFeature("CourseGradeB.EduAdminExtendControls.Ribbon.HRTConductStatusForm", "Conduct輸入狀況檢視(班導師)"));
            //ribbon.Add(new RibbonFeature("JHSchool.EduAdmin.Ribbon.ExamTemplateManager", "評分樣板設定"));

            //學務作業
            ribbon = RoleAclSource.Instance["學務作業"];
            ribbon.Add(new RibbonFeature("JHSchool.StuAdmin.Ribbon.ConductTitleManager", "指標管理"));
            #endregion

            #region 資料項目
            // 基本資料
            //Course.Instance.AddDetailBulider(new JHSchool.Legacy.ContentItemBulider<BasicInfoItem>());
            Course.Instance.AddDetailBulider(new JHSchool.Legacy.ContentItemBulider<BasicInfoItem>());

            // 修課學生
            Course.Instance.AddDetailBulider(new JHSchool.Legacy.ContentItemBulider<SCAttendItem>());

            // 學期成績
            Student.Instance.AddDetailBulider(new JHSchool.Legacy.ContentItemBulider<SemsSubjScoreItem>());

            #endregion

            #region 課程/編輯
            RibbonBarItem rbItem = Student.Instance.RibbonBarItems["教務"];
            RibbonBarButton rbButton;
            rbItem = Course.Instance.RibbonBarItems["編輯"];
            rbButton = rbItem["新增"];
            rbButton.Size = RibbonBarButton.MenuButtonSize.Large;
            rbButton.Image = Properties.Resources.btnAddCourse;
            rbButton.Enable = User.Acl["JHSchool.Course.Ribbon0000"].Executable;
            rbButton.Click += delegate
            {
                new CourseGradeB.CourseExtendControls.Ribbon.AddCourse().ShowDialog();
            };

            rbButton = rbItem["刪除"];
            rbButton.Size = RibbonBarButton.MenuButtonSize.Large;
            rbButton.Image = Properties.Resources.btnDeleteCourse;
            rbButton.Enable = User.Acl["JHSchool.Course.Ribbon0010"].Executable;
            rbButton.Click += delegate
            {
                if (Course.Instance.SelectedKeys.Count == 1)
                {
                    JHSchool.Data.JHCourseRecord record = JHSchool.Data.JHCourse.SelectByID(Course.Instance.SelectedKeys[0]);
                    //int CourseAttendCot = Course.Instance.Items[record.ID].GetAttendStudents().Count;
                    List<JHSchool.Data.JHSCAttendRecord> scattendList = JHSchool.Data.JHSCAttend.SelectByStudentIDAndCourseID(new List<string>() { }, new List<string>() { record.ID });
                    int attendStudentCount = 0;
                    foreach (JHSchool.Data.JHSCAttendRecord scattend in scattendList)
                    {
                        if (scattend.Student.Status == K12.Data.StudentRecord.StudentStatus.一般)
                            attendStudentCount++;
                    }

                    if (attendStudentCount > 0)
                        MsgBox.Show(record.Name + " 有" + attendStudentCount.ToString() + "位修課學生，請先移除修課學生後再刪除課程.");
                    else
                    {
                        string msg = string.Format("確定要刪除「{0}」？", record.Name);
                        if (MsgBox.Show(msg, "刪除課程", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            #region 自動刪除非一般學生的修課記錄
                            List<JHSchool.Data.JHSCAttendRecord> deleteSCAttendList = new List<JHSchool.Data.JHSCAttendRecord>();
                            foreach (JHSchool.Data.JHSCAttendRecord scattend in scattendList)
                            {
                                JHSchool.Data.JHStudentRecord stuRecord = JHSchool.Data.JHStudent.SelectByID(scattend.RefStudentID);
                                if (stuRecord == null) continue;
                                if (stuRecord.Status != K12.Data.StudentRecord.StudentStatus.一般)
                                    deleteSCAttendList.Add(scattend);
                            }
                            List<string> studentIDs = new List<string>();
                            foreach (JHSchool.Data.JHSCAttendRecord scattend in deleteSCAttendList)
                                studentIDs.Add(scattend.RefStudentID);
                            List<JHSchool.Data.JHSCETakeRecord> sceList = JHSchool.Data.JHSCETake.SelectByStudentAndCourse(studentIDs, new List<string>() { record.ID });
                            JHSchool.Data.JHSCETake.Delete(sceList);
                            JHSchool.Data.JHSCAttend.Delete(deleteSCAttendList);
                            #endregion

                            JHSchool.Data.JHCourse.Delete(record);

                            //刪除CourseExtendRecord
                            List<CourseExtendRecord> list = a.Select<CourseExtendRecord>("ref_course_id=" + record.ID);
                            if (list.Count > 0)
                                a.DeletedValues(list);

                            // 加這主要是重新整理
                            Course.Instance.SyncDataBackground(record.ID);
                        }
                        else
                            return;
                    }
                }
            };

            RibbonBarButton CouItem = Course.Instance.RibbonBarItems["編輯"]["刪除"];
            Course.Instance.SelectedListChanged += delegate
            {
                // 課程刪除不能多選
                CouItem.Enable = (Course.Instance.SelectedList.Count < 2) && User.Acl["JHSchool.Course.Ribbon0010"].Executable;
            };

            //指定評分樣板
            //CouItem = Course.Instance.RibbonBarItems["指定"]["指定評分樣板"];
            //Course.Instance.SelectedListChanged += delegate
            //{
            //    CouItem.Enable = (Course.Instance.SelectedList.Count > 0);
            //};
            //CouItem.Click += delegate
            //{
            //    new CourseGradeB.CourseExtendControls.Ribbon.GiveRefExamTemplateForm(K12.Presentation.NLDPanels.Course.SelectedSource).ShowDialog();
            //};

            RibbonBarItem scores = JHSchool.Course.Instance.RibbonBarItems["教務"];
            scores["成績輸入"].Size = RibbonBarButton.MenuButtonSize.Medium;
            scores["成績輸入"].Image = Properties.Resources.exam_write_64;
            scores["成績輸入"].Enable = Framework.User.Acl["JHSchool.Course.Ribbon0070.CourseScoreInputForm"].Executable;
            scores["成績輸入"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Course.SelectedSource.Count == 1)
                {
                    CourseRecord courseRecord = Course.Instance.SelectedList[0];
                    //int key = int.Parse(courseRecord.ID);
                    //int value = Global.Instance.CourseExtendCatch.ContainsKey(key) ? Global.Instance.CourseExtendCatch[key] : -1;
                    //if (value == -1)
                    //     FISCA.Presentation.Controls.MsgBox.Show("課程 '" + courseRecord.Name + "' 沒有評量設定。");
                    //else
                    new CourseGradeB.CourseExtendControls.Ribbon.CourseScoreInputForm(courseRecord).ShowDialog();
                }
            };
            K12.Presentation.NLDPanels.Course.SelectedSourceChanged += delegate
            {
                scores["成績輸入"].Enable = K12.Presentation.NLDPanels.Course.SelectedSource.Count == 1 && Framework.User.Acl["JHSchool.Course.Ribbon0070.CourseScoreInputForm"].Executable;
            };

            RibbonBarItem conduct = JHSchool.Course.Instance.RibbonBarItems["教務"];
            conduct["指標輸入"].Size = RibbonBarButton.MenuButtonSize.Medium;
            conduct["指標輸入"].Image = Properties.Resources.exam_write_64;
            conduct["指標輸入"].Enable = Framework.User.Acl["JHSchool.Course.Ribbon0070.SubjectConductInputForm"].Executable;
            conduct["指標輸入"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Course.SelectedSource.Count == 1)
                {
                    CourseRecord courseRecord = Course.Instance.SelectedList[0];
                    new CourseGradeB.CourseExtendControls.Ribbon.SubjectConductInputForm(courseRecord).ShowDialog();
                }
            };
            K12.Presentation.NLDPanels.Course.SelectedSourceChanged += delegate
            {
                conduct["指標輸入"].Enable = K12.Presentation.NLDPanels.Course.SelectedSource.Count == 1 && Framework.User.Acl["JHSchool.Course.Ribbon0070.SubjectConductInputForm"].Executable;
            };

            RibbonBarItem editGrade = JHSchool.Course.Instance.RibbonBarItems["指定"];
            editGrade["批次修改開課年級"].Size = RibbonBarButton.MenuButtonSize.Medium;
            editGrade["批次修改開課年級"].Image = Properties.Resources.record_b_write_64;
            editGrade["批次修改開課年級"].Enable = Framework.User.Acl["JHSchool.Course.Ribbon0070.CourseGradeEditor"].Executable;
            editGrade["批次修改開課年級"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Course.SelectedSource.Count > 0)
                {
                    new CourseGradeB.CourseExtendControls.Ribbon.CourseGradeEditor(K12.Presentation.NLDPanels.Course.SelectedSource).ShowDialog();
                }
            };
            K12.Presentation.NLDPanels.Course.SelectedSourceChanged += delegate
            {
                editGrade["批次修改開課年級"].Enable = K12.Presentation.NLDPanels.Course.SelectedSource.Count > 0 && Framework.User.Acl["JHSchool.Course.Ribbon0070.CourseGradeEditor"].Executable;
            };

            #endregion

            #region 匯出/匯入

            RibbonBarButton rbItemExport = Student.Instance.RibbonBarItems["資料統計"]["匯出"];
            RibbonBarButton rbItemImport = Student.Instance.RibbonBarItems["資料統計"]["匯入"];

            rbItemExport["成績相關匯出"]["匯出學期歷程"].Enable = User.Acl["JHSchool.Student.Ribbon0169"].Executable;
            rbItemExport["成績相關匯出"]["匯出學期歷程"].Click += delegate
            {
                SmartSchool.API.PlugIn.Export.Exporter exporter = new CourseGradeB.ImportExport.ExportSemesterHistory();
                CourseGradeB.ImportExport.ExportStudentV2 wizard = new CourseGradeB.ImportExport.ExportStudentV2(exporter.Text, exporter.Image);
                exporter.InitializeExport(wizard);
                wizard.ShowDialog();
            };

            rbItemImport["成績相關匯入"]["匯入學期歷程"].Enable = User.Acl["JHSchool.Student.Ribbon0170"].Executable;
            rbItemImport["成績相關匯入"]["匯入學期歷程"].Click += delegate
            {
                SmartSchool.API.PlugIn.Import.Importer importer = new CourseGradeB.ImportExport.ImportSemesterHistory();
                CourseGradeB.ImportExport.ImportStudentV2 wizard = new CourseGradeB.ImportExport.ImportStudentV2(importer.Text, importer.Image);
                importer.InitializeImport(wizard);
                wizard.ShowDialog();
            };

            RibbonBarItem rbItemCourseImportExport = Course.Instance.RibbonBarItems["資料統計"];
            rbItemCourseImportExport["匯出"]["匯出課程修課學生"].Enable = User.Acl["JHSchool.Course.Ribbon0031"].Executable;
            rbItemCourseImportExport["匯出"]["匯出課程修課學生"].Click += delegate
            {
                SmartSchool.API.PlugIn.Export.Exporter exporter = new CourseGradeB.ImportExport.Course.ExportCourseStudents("");
                CourseGradeB.ImportExport.Course.ExportStudentV2 wizard = new CourseGradeB.ImportExport.Course.ExportStudentV2(exporter.Text, exporter.Image);
                exporter.InitializeExport(wizard);
                wizard.ShowDialog();
            };
            rbItemCourseImportExport["匯入"]["匯入課程修課學生"].Enable = User.Acl["JHSchool.Course.Ribbon0021"].Executable;
            rbItemCourseImportExport["匯入"]["匯入課程修課學生"].Click += delegate
            {
                SmartSchool.API.PlugIn.Import.Importer importer = new CourseGradeB.ImportExport.Course.ImportCourseStudents("");
                CourseGradeB.ImportExport.Course.ImportStudentV2 wizard = new CourseGradeB.ImportExport.Course.ImportStudentV2(importer.Text, importer.Image);
                importer.InitializeImport(wizard);
                wizard.ShowDialog();
            };

            #endregion

            #region 班級功能

            rbButton = K12.Presentation.NLDPanels.Class.RibbonBarItems["教務"]["班級開課"];
            rbButton.Enable = User.Acl["JHSchool.Class.Ribbon0070"].Executable;
            rbButton.Image = Properties.Resources.organigram_refresh_64;
            rbButton["直接開課"].Click += delegate
            {
                if (Class.Instance.SelectedList.Count > 0)
                    new CourseGradeB.ClassExtendControls.Ribbon.CreateCoursesDirectly();
            };

            RibbonBarItem hrtConduct = JHSchool.Class.Instance.RibbonBarItems["教務"];
            hrtConduct["指標輸入"].Size = RibbonBarButton.MenuButtonSize.Medium;
            hrtConduct["指標輸入"].Image = Properties.Resources.exam_write_64;
            hrtConduct["指標輸入"].Enable = Framework.User.Acl["JHSchool.Class.Ribbon0070.HrtConductInputForm"].Executable;
            hrtConduct["指標輸入"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Class.SelectedSource.Count == 1)
                {
                    new CourseGradeB.ClassExtendControls.Ribbon.HrtSelectSchoolYear(K12.Presentation.NLDPanels.Class.SelectedSource[0]).ShowDialog();
                }
            };
            K12.Presentation.NLDPanels.Class.SelectedSourceChanged += delegate
            {
                hrtConduct["指標輸入"].Enable = K12.Presentation.NLDPanels.Class.SelectedSource.Count == 1 && Framework.User.Acl["JHSchool.Class.Ribbon0070.HrtConductInputForm"].Executable;
            };

            #endregion

            #region 教務作業功能
            FISCA.Presentation.RibbonBarItem eduitem1 = FISCA.Presentation.MotherForm.RibbonBarItems["教務作業", "基本設定"];
            eduitem1["對照/代碼"].Image = Properties.Resources.notepad_lock_64;
            eduitem1["對照/代碼"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Large;

            FISCA.Presentation.RibbonBarItem eduitem2 = FISCA.Presentation.MotherForm.RibbonBarItems["教務作業", "批次作業/檢視"];
            eduitem2["成績作業"].Image = Properties.Resources.calc_save_64;
            eduitem2["成績作業"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Large;

            eduitem2["成績作業"]["批次計算科目成績"].Enable = User.Acl["CourseGradeB.EduAdminExtendControls.Ribbon.SubjectScoreCalculateByGradeyear"].Executable;
            eduitem2["成績作業"]["批次計算科目成績"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.SubjectScoreCalculateByGradeyear().ShowDialog();
            };

            eduitem2["成績作業"]["歷屆GPA統計"].Enable = User.Acl["CourseGradeB.EduAdminExtendControls.Ribbon.GpaRefForm"].Executable;
            eduitem2["成績作業"]["歷屆GPA統計"].Click += delegate
            {
                new GpaRefForm().ShowDialog();
            };

            //產生學期歷程
            eduitem2["成績作業"]["產生學期歷程"].Enable = User.Acl["CourseGradeB.EduAdminExtendControls.Ribbon.SemsHistoryMaker"].Executable;

            eduitem2["成績作業"]["產生學期歷程"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.SemsHistoryMaker().ShowDialog();
            };

            //評量輸入狀況檢視
            eduitem2["成績作業"]["評量輸入狀況檢視"].Enable = User.Acl["CourseGradeB.EduAdminExtendControls.Ribbon.CourseScoreStatusForm"].Executable;

            eduitem2["成績作業"]["評量輸入狀況檢視"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.CourseScoreStatusForm().ShowDialog();
            };

            //Conduct輸入狀況檢視(授課老師)
            eduitem2["成績作業"]["Conduct輸入狀況檢視(授課老師)"].Enable = User.Acl["CourseGradeB.EduAdminExtendControls.Ribbon.SubjectConductStatusForm"].Executable;

            eduitem2["成績作業"]["Conduct輸入狀況檢視(授課老師)"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.SubjectConductStatusForm().ShowDialog();
            };

            //Conduct輸入狀況檢視(班導師)
            eduitem2["成績作業"]["Conduct輸入狀況檢視(班導師)"].Enable = User.Acl["CourseGradeB.EduAdminExtendControls.Ribbon.HRTConductStatusForm"].Executable;

            eduitem2["成績作業"]["Conduct輸入狀況檢視(班導師)"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.HRTConductStatusForm().ShowDialog();
            };

            RibbonBarItem eduitem3 = EduAdmin.Instance.RibbonBarItems["基本設定"];
            eduitem3["管理"].Size = RibbonBarButton.MenuButtonSize.Large;
            eduitem3["管理"].Image = Properties.Resources.network_lock_64;

            //rbItem["管理"]["評量名稱管理"].Enable = User.Acl["JHSchool.EduAdmin.Ribbon0000"].Executable;
            //rbItem["管理"]["評量名稱管理"].Click += delegate
            //{
            //    new CourseGradeB.CourseExtendControls.Ribbon.ExamManager().ShowDialog();
            //};

            eduitem3["管理"]["科目資料管理"].Enable = User.Acl["JHSchool.EduAdmin.Ribbon.SubjectManager"].Executable;
            eduitem3["管理"]["科目資料管理"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.SubjectManager().ShowDialog();
            };

            eduitem3["管理"]["開放時間管理"].Enable = User.Acl["CourseGradeB.EduAdminExtendControls.Ribbon.SetHoursOpeningForm"].Executable;
            eduitem3["管理"]["開放時間管理"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.SetHoursOpeningForm().ShowDialog();
            };

            //rbItem["設定"].Image = Properties.Resources.sandglass_unlock_64;
            //rbItem["設定"].Size = RibbonBarButton.MenuButtonSize.Large;
            //rbItem["設定"]["評分樣板設定"].Enable = User.Acl["JHSchool.EduAdmin.Ribbon.ExamTemplateManager"].Executable;
            //rbItem["設定"]["評分樣板設定"].Click += delegate
            //{
            //    new CourseGradeB.EduAdminExtendControls.Ribbon.ExamTemplateManager().ShowDialog();
            //};

            #endregion

            #region 學務作業功能
            FISCA.Presentation.RibbonBarItem stuitem1 = FISCA.Presentation.MotherForm.RibbonBarItems["學務作業", "基本設定"];
            //item3["管理"].Image = Properties.Resources.network_lock_64;
            //item3["管理"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Large;
            stuitem1["管理"]["指標管理"].Enable = User.Acl["JHSchool.StuAdmin.Ribbon.ConductTitleManager"].Executable;
            stuitem1["管理"]["指標管理"].Click += delegate
            {
                new CourseGradeB.StuAdminExtendControls.ConductSettingForm().ShowDialog();
            };
            #endregion

            #region 學生功能
            //註冊成績計算功能項目。
            FISCA.Presentation.RibbonBarItem student_rbitem = FISCA.Presentation.MotherForm.RibbonBarItems["學生", "教務"];
            student_rbitem["成績作業"].Size = RibbonBarButton.MenuButtonSize.Large;
            student_rbitem["成績作業"].Image = Properties.Resources.calc_save_64;
            student_rbitem["成績作業"]["計算科目成績"].Enable = false;
            K12.Presentation.NLDPanels.Student.SelectedSourceChanged += delegate
            {
                student_rbitem["成績作業"]["計算科目成績"].Enable = K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0 && User.Acl["JHSchool.Student.SubjectScoreCalculate"].Executable;
            };

            student_rbitem["成績作業"]["計算科目成績"].Click += delegate
            {
                new CourseGradeB.StudentExtendControls.Ribbon.SubjectScoreCalculate(K12.Presentation.NLDPanels.Student.SelectedSource).ShowDialog(); 
            };
            #endregion

            //教師系統類別
            Tagging.Main();
            ResCourseData();
        }

        private static void ResCourseData()
        {
            //課程科目屬性
            ListPaneField CourseSubjectType = new ListPaneField("科目組別");
            CourseSubjectType.PreloadVariable += delegate(object sender, PreloadVariableEventArgs e)
            {
                Tool.Instance.Courses = K12.Data.Course.SelectByIDs(e.Keys);
                Tool.Instance.Refresh();
            };

            CourseSubjectType.GetVariable += delegate(object sender, GetVariableEventArgs e)
            {
                e.Value = Tool.Instance.GetSubjectType(e.Key);
            };

            K12.Presentation.NLDPanels.Course.AddListPaneField(CourseSubjectType);

            //FISCA.InteractionService.SubscribeEvent("SubjectChange", (sender, args) =>
            //{
            //    //取得更新資料
            //    Tool.Instance.Refresh();
            //    //重讀
            //    CourseSubjectType.Reload();
            //});
        }
    }
}
