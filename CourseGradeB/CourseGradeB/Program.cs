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


namespace CourseGradeB
{
    public class Program
    {
        [MainMethod("")]
        public static void Main()
        {
            // 課程加入教師檢視
            Course.Instance.AddView(new TeacherCategoryView());

            #region 權限註冊
            // 學生
            Catalog ribbon = RoleAclSource.Instance["學生"]["功能按鈕"];
            ribbon.Add(new RibbonFeature("JHSchool.Student.Ribbon0169", "匯出學期歷程"));
            ribbon.Add(new RibbonFeature("JHSchool.Student.Ribbon0170", "匯入學期歷程"));

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

            //教務作業
            ribbon = RoleAclSource.Instance["教務作業"];
            //ribbon.Add(new RibbonFeature("JHSchool.EduAdmin.Ribbon0000", "評量名稱管理"));
            ribbon.Add(new RibbonFeature("JHSchool.EduAdmin.Ribbon.SubjectManager", "科目資料管理"));
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
                
            #endregion

            #region 匯出/匯入

            RibbonBarButton rbItemExport = Student.Instance.RibbonBarItems["資料統計"]["匯出"];
            RibbonBarButton rbItemImport = Student.Instance.RibbonBarItems["資料統計"]["匯入"];

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
            FISCA.Presentation.RibbonBarItem item1 = FISCA.Presentation.MotherForm.RibbonBarItems["教務作業", "基本設定"];
            item1["對照/代碼"].Image = Properties.Resources.notepad_lock_64;
            item1["對照/代碼"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Large;

            FISCA.Presentation.RibbonBarItem item2 = FISCA.Presentation.MotherForm.RibbonBarItems["教務作業", "批次作業/檢視"];
            item2["成績作業"].Image = Properties.Resources.calc_save_64;
            item2["成績作業"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Large;

            rbItem = EduAdmin.Instance.RibbonBarItems["基本設定"];
            rbItem["管理"].Size = RibbonBarButton.MenuButtonSize.Large;
            rbItem["管理"].Image = Properties.Resources.network_lock_64;

            //rbItem["管理"]["評量名稱管理"].Enable = User.Acl["JHSchool.EduAdmin.Ribbon0000"].Executable;
            //rbItem["管理"]["評量名稱管理"].Click += delegate
            //{
            //    new CourseGradeB.CourseExtendControls.Ribbon.ExamManager().ShowDialog();
            //};

            rbItem["管理"]["科目資料管理"].Enable = User.Acl["JHSchool.EduAdmin.Ribbon.SubjectManager"].Executable;
            rbItem["管理"]["科目資料管理"].Click += delegate
            {
                new CourseGradeB.EduAdminExtendControls.Ribbon.SubjectManager().ShowDialog();
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
            FISCA.Presentation.RibbonBarItem item3 = FISCA.Presentation.MotherForm.RibbonBarItems["學務作業", "基本設定"];
            //item3["管理"].Image = Properties.Resources.network_lock_64;
            //item3["管理"].Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Large;
            item3["管理"]["指標管理"].Enable = User.Acl["JHSchool.StuAdmin.Ribbon.ConductTitleManager"].Executable;
            item3["管理"]["指標管理"].Click += delegate
            {
                new CourseGradeB.StuAdminExtendControls.ConductSettingForm().ShowDialog();
            };
            #endregion
            //ResCourseData();
        }

        //private static void ResCourseData()
        //{
        //    //評分樣板顯示
        //    ListPaneField CourseRefExamTemplate = new ListPaneField("評分樣板");
            
        //    CourseRefExamTemplate.GetVariable += delegate(object sender, GetVariableEventArgs e)
        //    {
        //        int key;
        //        bool done = int.TryParse(e.Key,out key);
        //        if(done)
        //        {
        //            e.Value = Global.Instance.GetExamTemplateName(key);
        //        }
        //    };
        //    K12.Presentation.NLDPanels.Course.AddListPaneField(CourseRefExamTemplate);

        //    FISCA.InteractionService.SubscribeEvent("Res_CourseExt", (sender, args) =>
        //    {
        //        //取得更新資料
        //        Global.Instance.Refresh();
        //        //重讀
        //        CourseRefExamTemplate.Reload();
        //    });
        //}
    }
}
