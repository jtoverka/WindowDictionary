using WindowDictionary.Property.Editor;
using WindowDictionary.Property.Creator;
using System.Windows;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Select the property type from either property creator or property editor
    /// </summary>
    public class PropertyType : DependencyObject
    {
        #region Properties
        #region Property - CPropertyType : CPropertyType

        /// <summary>
        /// Get or Set the property creator property type
        /// </summary>
        public CPropertyType CPropertyType
        {
            get { return (CPropertyType)GetValue(CPropertyProperty); }
            set { SetValue(CPropertyProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CProperty.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CPropertyProperty =
            DependencyProperty.Register("CPropertyType", typeof(CPropertyType), typeof(PropertyType));

        #endregion
        #region Property - EPropertyType : EPropertyType

        /// <summary>
        /// Get or Set the property editor property type
        /// </summary>
        public EPropertyType EPropertyType
        {
            get { return (EPropertyType)GetValue(EPropertyTypeProperty); }
            set { SetValue(EPropertyTypeProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for EPropertyType.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty EPropertyTypeProperty =
            DependencyProperty.Register("EPropertyType", typeof(EPropertyType), typeof(PropertyType));

        #endregion
        #endregion
    }
}