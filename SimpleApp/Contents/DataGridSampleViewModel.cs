using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using uEN.Core;
using uEN.UI;
using uEN.UI.AttachedProperties;
using uEN.UI.Validation;

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(DataGridSampleView))]
    public class DataGridSampleViewModel : BizViewModel
    {
        public override string Description { get { return "DataGrid Sample"; } }


        public override void LoadedView()
        {
            if (Initialized) return;

            SimpleGrid = this.CreateSimpleGrid(CreateSampleData());
            SimpleGrid.GridSource.Filter = gridFilter;
        }

        private static IEnumerable<SampleGridModel> CreateSampleData()
        {
            var list = new List<SampleGridModel>();
            for (int i = 0; i < 101; i++)
            {
                var item = new SampleGridModel()
                {
                    Identity = string.Format("ID - {0}", i),
                    Check = false,
                    Value = i,
                    Dt = DateTime.Now.AddDays(i),
                    Items = new List<SampleGridModel.childClass>()
                    {
                        new SampleGridModel.childClass() { Identity = 1, Title = "Title - 1" },
                        new SampleGridModel.childClass() { Identity = 2, Title = "Title - 2" },
                        new SampleGridModel.childClass() { Identity = 3, Title = "Title - 3" },
                    }
                };
                list.Add(item);
            }
            return list;
        }

        public ISimpleGrid SimpleGrid { get; set; }

        bool gridFilter(object obj)
        {
            var item = obj as SampleGridModel;
            if (item == null) return true;

            if (string.IsNullOrWhiteSpace(SearchIdentity)) return true;

            return item.Identity.Contains(SearchIdentity.ToUpper());
        }

        [TextInputPolicy(TextInputState.Disable)]
        [AlphanumericAnnotation]
        public string SearchIdentity { get; set; }
        public void FindAction()
        {
            SimpleGrid.GridSource.Refresh();
        }
        public void ClearAction()
        {
            SearchIdentity = null;
            SimpleGrid.GridSource.Refresh();
            UpdateTarget();
        }

    }



    public class SampleGridModel
    {
        [DataGridColumnAnnotation(10, "Identity", 80d)]
        public string Identity { get; set; }

        [DataGridColumnAnnotation(20, "CheckBox Colmun", 250d, IsReadOnly = false)]
        public bool Check { get; set; }

        [NumericAnnotation(5, 2)]
        [DataGridColumnAnnotation(30, "Numeric Colmun", 250d, IsReadOnly = false)]
        public decimal? Value { get; set; }

        [DataGridColumnAnnotation(40, "DatePicker Colmun", 250d, IsReadOnly = false)]
        public DateTime? Dt { get; set; }

        [DataGridColumnAnnotation(50, "ComboBox Colmun", 250d, IsReadOnly = false, ComboBoxDisplayMemberPath = "Title", ComboBoxItemsSourcePath = "Items")]
        public childClass SelectedText { get; set; }

        public IEnumerable<childClass> Items { get; set; }


        public class childClass
        {
            public int Identity { get; set; }
            public string Title { get; set; }
        }
    }

}
