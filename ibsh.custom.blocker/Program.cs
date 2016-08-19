using FISCA.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ibsh.custom.blocker
{
    public class Program
    {
        [FISCA.MainMethod]
        public static void main()
        {
            {
                Catalog permission = RoleAclSource.Instance["教務作業"]["學生家長查詢"];
                permission.Add(new RibbonFeature("{9C50900E-56C2-4ADC-A53B-DAE771D2F0EF}", "關閉線上查詢"));

                FISCA.Presentation.RibbonBarItem eduitem1 = FISCA.Presentation.MotherForm.RibbonBarItems["教務作業", "學生家長查詢"];
                var btn = eduitem1["關閉線上查詢"];
                btn.Size = FISCA.Presentation.RibbonBarButton.MenuButtonSize.Large;
                btn.Enable = FISCA.Permission.UserAcl.Current["{9C50900E-56C2-4ADC-A53B-DAE771D2F0EF}"].Executable;
                btn.Click += delegate
                {
                    new BlockConfig().ShowDialog();
                };
            }

        }
    }
}
