using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Performs a logical operation 
    /// </summary>
    public class LogicalGate : BooleanRange, INotifyPropertyChanged
    {
        #region Properties

        private LogicalOperator _Operator;
        /// <summary>
        /// 
        /// </summary>
        public LogicalOperator Operator
        {
            get { return this._Operator; }
            protected set
            {
                if (this._Operator == value)
                    return;

                this._Operator = value;
                OnPropertyChanged("Operator");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Range> RangeCollection { get; } = new ObservableCollection<Range>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <c>LogicalGate</c> class.
        /// </summary>
        /// <param name="logicalOperator"><see cref="LogicalOperator">Logical Operator</see></param>
        public LogicalGate(LogicalOperator logicalOperator)
        {
            this.Operator = logicalOperator;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Performs the Logical Operation specified by <see cref="Operator">Operator</see> on <see cref="RangeCollection">RangeCollection</see>.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            bool output = false;
            int count = 0;

            switch (this.Operator)
            {
                case LogicalOperator.AND:
                    output = true;
                    foreach (Range range in RangeCollection)
                    {
                        output &= range.IsValid(value);
                    }
                    break;
                case LogicalOperator.NAND:
                    output = true;
                    foreach (Range range in RangeCollection)
                    {
                        output &= range.IsValid(value);
                    }
                    output = !output;
                    break;
                case LogicalOperator.NOR:
                    output = false;
                    foreach (Range range in RangeCollection)
                    {
                        output |= range.IsValid(value);
                    }
                    output = !output;
                    break;
                case LogicalOperator.NOT:
                    output = !RangeCollection[0].IsValid(value);
                    break;
                case LogicalOperator.OR:
                    output = false;
                    foreach (Range range in RangeCollection)
                    {
                        output |= range.IsValid(value);
                    }
                    break;
                case LogicalOperator.XOR:
                    foreach (Range range in RangeCollection)
                    {
                        if (range.IsValid(value))
                            count++;
                    }
                    output = (count % 2) == 1;
                    break;
                case LogicalOperator.XNOR:
                    foreach (Range range in RangeCollection)
                    {
                        if (range.IsValid(value))
                            count++;
                    }
                    output = ((count % 2) == 0);
                    break;
            }

            return output;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked when a Component Property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes PropertyChanged Event
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
