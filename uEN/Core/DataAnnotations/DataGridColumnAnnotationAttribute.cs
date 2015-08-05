using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using uEN.UI.Validation;

namespace uEN.Core
{

    public class DataGridColumnAnnotationAttribute : Attribute
    {

        public DataGridColumnAnnotationAttribute(int identity, string header)
        {
            Idntity = identity;
            Header = header;
        }
        public DataGridColumnAnnotationAttribute(int identity, string header, double size)
            : this(identity, header, size, true)
        {
        }
        public DataGridColumnAnnotationAttribute(int identity, string header, double size, bool isReadOnly)
            : this(identity, header, size, isReadOnly, null)
        {
        }
        public DataGridColumnAnnotationAttribute(int identity, string header, double size, bool isReadOnly, string stringFormat)
            : this(identity, header, size, isReadOnly, stringFormat, false)
        {
        }
        public DataGridColumnAnnotationAttribute(int identity, string header, double size, bool isReadOnly, string stringFormat, bool isSizeStar)
            : this(identity, header, size, isReadOnly, stringFormat, isSizeStar, null, null)
        {
        }
        public DataGridColumnAnnotationAttribute(int identity, string header, double size, bool isReadOnly, string stringFormat, bool isSizeStar
            , string comboBoxItemsSourcePath, string comboBoxDisplayMemberPath)
        {
            Idntity = identity;
            Header = header;
            Size = size;
            IsReadOnly = isReadOnly;
            StringFormat = stringFormat;
            IsStar = isSizeStar;
            ComboBoxDisplayMemberPath = comboBoxDisplayMemberPath;
            ComboBoxItemsSourcePath = comboBoxItemsSourcePath;
        }

        public int Idntity { get; set; }
        public string Header { get; set; }
        public double? Size { get; set; }
        public bool IsStar { get; set; }
        public string StringFormat { get; set; }

        public bool IsAutoSize
        {
            get { return _IsAutoSize; }
            set { _IsAutoSize = value; }
        }
        bool _IsAutoSize = false;

        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set { _IsReadOnly = value; }
        }
        bool _IsReadOnly = true;

        public bool CanUserResize
        {
            get { return _CanUserResize; }
            set { _CanUserResize = value; }
        }
        bool _CanUserResize = true;

        public bool CanUserSort
        {
            get { return _CanUserSort; }
            set { _CanUserSort = value; }
        }
        bool _CanUserSort = true;

        public bool CanUserReorder
        {
            get { return _CanUserReorder; }
            set { _CanUserReorder = value; }
        }
        bool _CanUserReorder = true;

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }
        bool _IsVisible = true;

        public string ComboBoxItemsSourcePath { get; set; }
        public string ComboBoxDisplayMemberPath { get; set; }

        public ItemPropertyInfo PropertyInfo { get; internal set; }
        public IList<ValidationRule> Validations { get; internal set; }
    }

    public interface IDataGridColumnFactory
    {
        DataGridColumn Create(DataGridColumnAnnotationAttribute att);
    }

    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IDataGridColumnFactory))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    public class DefaultDataGridColumnFactory : IDataGridColumnFactory
    {
        protected static readonly Type[] numericTypes = new Type[] { 
            typeof(int) ,
            typeof(Int16) ,typeof(Int32) ,typeof(Int64),   
            typeof(UInt16) ,typeof(UInt32) ,typeof(UInt64),   
            typeof(decimal) , typeof(float) ,typeof(double) ,typeof(Single)
        };
        public virtual DataGridColumn Create(DataGridColumnAnnotationAttribute att)
        {
            DataGridColumn column = null;

            att.Validations = new List<ValidationRule>();
            var descriptor = (PropertyDescriptor)att.PropertyInfo.Descriptor;
            foreach (var each in descriptor.Attributes.OfType<ValidationAttribute>())
            {
                var rule = new DataAnnotationRule(each);
                att.Validations.Add(rule);
            }

            var type = GetType(att.PropertyInfo.PropertyType);
            if (!string.IsNullOrWhiteSpace(att.ComboBoxItemsSourcePath))
            {
                column = CreateComboBoxColumn(att);
            }
            else if (type == typeof(bool))
            {
                column = CreateCheckBoxColumn(att);
            }
            else if (type == typeof(DateTime))
            {
                column = CreateDatePickerColumn(att);
            }
            else if (numericTypes.Contains(type))
            {
                column = CreateNumericColumn(att);
            }
            else
            {
                column = CreateDefaultColumn(att);
            }

            if (att.IsStar)
            {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            else if (att.IsAutoSize)
            {
                column.Width = DataGridLength.Auto;
            }
            else
            {
                column.Width = att.Size.Value;
            }
            column.Header = att.Header;
            column.IsReadOnly = att.IsReadOnly;
            column.CanUserResize = att.CanUserResize;
            column.CanUserSort = att.CanUserSort;
            column.CanUserReorder = att.CanUserReorder;
            column.Visibility = att.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            return column;
        }

        protected virtual DataGridColumn CreateComboBoxColumn(DataGridColumnAnnotationAttribute att)
        {
            var column = new DataGridTemplateColumn();
            if (att.IsReadOnly)
            {
                var binding = CreateBinding(att);
                binding.Path = new PropertyPath(string.Format("{0}.{1}", att.PropertyInfo.Name, att.ComboBoxDisplayMemberPath));
                var template = new DataTemplate()
                {
                    VisualTree = new FrameworkElementFactory(typeof(TextBlock))
                };
                template.VisualTree.SetBinding(TextBlock.TextProperty, binding);
                template.VisualTree.SetValue(TextBlock.PaddingProperty, new Thickness(2, 0, 0, 0));
                template.VisualTree.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                template.VisualTree.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                column.CellTemplate = template;
                return column;
            }

            var selectedBinding = CreateBinding(att);
            if (!att.IsReadOnly)
            {
                selectedBinding.Mode = BindingMode.TwoWay;
                selectedBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            }

            var itemsSourceBinding = CreateBinding(att);
            itemsSourceBinding.Path = new PropertyPath(att.ComboBoxItemsSourcePath);

            var editingTemplate = new DataTemplate()
            {
                VisualTree = new FrameworkElementFactory(typeof(ComboBox))
            };
            editingTemplate.VisualTree.SetBinding(ComboBox.SelectedItemProperty, selectedBinding);
            editingTemplate.VisualTree.SetBinding(ComboBox.ItemsSourceProperty, itemsSourceBinding);
            editingTemplate.VisualTree.SetValue(ComboBox.DisplayMemberPathProperty, att.ComboBoxDisplayMemberPath);
            editingTemplate.VisualTree.SetValue(ComboBox.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            editingTemplate.VisualTree.SetValue(ComboBox.VerticalAlignmentProperty, VerticalAlignment.Center);
            column.CellTemplate = column.CellEditingTemplate = editingTemplate;
            column.SortMemberPath = string.Format("{0}.{1}", att.PropertyInfo.Name, att.ComboBoxDisplayMemberPath);
            return column;
        }
        protected virtual DataGridColumn CreateCheckBoxColumn(DataGridColumnAnnotationAttribute att)
        {
            var column = new DataGridCheckBoxColumn();

            //シングルクリック対応
            // http://blogs.msdn.com/b/vinsibal/archive/2008/08/27/more-datagrid-samples-custom-sorting-drag-and-drop-of-rows-column-selection-and-single-click-editing.aspx
            var baseStyle = System.Windows.Application.Current.Resources[typeof(DataGridCell)] as Style;
            var style = new Style(typeof(DataGridCell), baseStyle);
            style.Setters.Add(
                new EventSetter(DataGridCell.PreviewMouseLeftButtonDownEvent,
                                new System.Windows.Input.MouseButtonEventHandler(OnDataGridCellPreviewMouseLeftButtonDown)));
            column.CellStyle = style;

            //バインディング
            var binding = CreateBinding(att);
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            column.Binding = binding;

            var checkBoxStyle = System.Windows.Application.Current.Resources[typeof(CheckBox)] as Style;
            var editingStyle = new Style(typeof(CheckBox), checkBoxStyle);
            editingStyle.Setters.Add(new Setter(CheckBox.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            editingStyle.Setters.Add(new Setter(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center));
            column.EditingElementStyle = editingStyle;

            var readOnlyStyle = new Style(typeof(CheckBox), checkBoxStyle);
            readOnlyStyle.Setters.Add(new Setter(CheckBox.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            readOnlyStyle.Setters.Add(new Setter(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center));
            if (att.IsReadOnly)
                readOnlyStyle.Setters.Add(new Setter(CheckBox.IsEnabledProperty, false));
            readOnlyStyle.Setters.Add(new Setter(CheckBox.OpacityProperty, 1d));

            column.ElementStyle = readOnlyStyle;

            return column;
        }
        private static void OnDataGridCellPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cell = sender as DataGridCell;
            if (cell.IsReadOnly) return;

            var grid = cell.FindVisualParent<DataGrid>();
            var column = cell.Column;
            if (grid.IsReadOnly || column.IsReadOnly) return;

            if (!cell.IsEditing)
            {
                if (!cell.IsFocused)
                    cell.Focus();

                if (grid.SelectionUnit == DataGridSelectionUnit.FullRow)
                {
                    var row = cell.FindVisualParent<DataGridRow>();
                    if (!row.IsFocused)
                        row.Focus();
                    row.IsSelected = true;
                }
                else
                {
                    cell.IsSelected = true;
                }
            }
        }

        protected virtual DataGridColumn CreateDatePickerColumn(DataGridColumnAnnotationAttribute att)
        {
            var column = new DataGridTemplateColumn();

            var editingBinding = CreateBinding(att);
            editingBinding.StringFormat = null;
            var editingTemplate = new DataTemplate()
            {
                VisualTree = new FrameworkElementFactory(typeof(DatePicker))
            };
            editingTemplate.VisualTree.SetBinding(DatePicker.SelectedDateProperty, editingBinding);
            editingTemplate.VisualTree.SetValue(DatePicker.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            editingTemplate.VisualTree.SetValue(DatePicker.VerticalAlignmentProperty, VerticalAlignment.Center);
            column.CellEditingTemplate = editingTemplate;


            var binding = CreateBinding(att);
            if (string.IsNullOrWhiteSpace(binding.StringFormat))
            {
                binding.StringFormat = "yyyy/MM/dd";
            }
            var template = new DataTemplate()
            {
                VisualTree = new FrameworkElementFactory(typeof(TextBlock))
            };
            template.VisualTree.SetBinding(TextBlock.TextProperty, binding);
            template.VisualTree.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);

            template.VisualTree.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            column.CellTemplate = template;

            column.SortMemberPath = binding.Path.Path;
            return column;
        }
        protected virtual DataGridColumn CreateNumericColumn(DataGridColumnAnnotationAttribute att)
        {
            var column = new DataGridTextColumn();
            var binding = CreateBinding(att);
            if (IsNullbale(att.PropertyInfo.PropertyType))
            {
                var converter = new uEN.UI.DataBinding.SimpleValueConverter();
                converter.ConvertMethod = (arg1, arg2, arg3, arg4) => arg1;
                converter.ConvertBackMethod = (arg1, arg2, arg3, arg4) =>
                {
                    var s = Convert.ToString(arg1);
                    if (string.IsNullOrWhiteSpace(s)) return null;
                    return arg1;
                };
                binding.Converter = converter;
            }
            column.Binding = binding;


            column.ElementStyle =
                System.Windows.Application.Current.Resources["DataGridNumericColumnStyle"] as Style;
            column.EditingElementStyle =
                System.Windows.Application.Current.Resources["DataGridNumericColumnEditingStyle"] as Style;

            return column;
        }
        protected virtual DataGridColumn CreateDefaultColumn(DataGridColumnAnnotationAttribute att)
        {
            var column = new DataGridTextColumn();
            column.Binding = CreateBinding(att);

            column.ElementStyle =
                System.Windows.Application.Current.Resources["DataGridDefaultColumnStyle"] as Style;
            column.EditingElementStyle =
                System.Windows.Application.Current.Resources["DataGridDefaultColumnEditingStyle"] as Style;

            return column;
        }
        protected virtual Binding CreateBinding(DataGridColumnAnnotationAttribute att)
        {
            var binding = new Binding(att.PropertyInfo.Name);
            foreach (var each in att.Validations)
            {
                binding.ValidationRules.Add(each);
            }
            if (!string.IsNullOrWhiteSpace(att.StringFormat))
            {
                binding.StringFormat = att.StringFormat;
            }
            return binding;
        }
        protected virtual Type GetType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments().FirstOrDefault();
            }
            return type;
        }
        protected virtual bool IsNullbale(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }


}
