using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataStreamTest
{
    public partial class Form1 : Form
    {
        SortableBindingList<Trade> trades_db = new SortableBindingList<Trade>();

        static decimal ask;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);
            //dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer add_trade = new Timer();
            add_trade.Interval = 5000;
            add_trade.Start();

            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Start();

            timer.Tick += new EventHandler(ticks);

            timer.Tick += new EventHandler(ticks);
            add_trade.Tick += new EventHandler(add_trade_event);
            comboBox_pair.SelectedIndex = 0;

        }
        int id = 100100100;
        void ticks(object sender, EventArgs e)
        {           
            update_upl();
            update_ui();
        }

        void add_trade_event(object sender, EventArgs e)
        {
            //buy();
        }

        void buy()
        {
            id++;
            //Trade trade = new Trade(id, numericUpDown1.Value, numericUpDown2.Value, numericUpDown3.Value, false);
            string pair = (string)comboBox_pair.SelectedItem;
            Trade trade = new Trade(id, pair, numericUpDown1.Value, numericUpDown2.Value, numericUpDown3.Value, 0);
            trades_db.Add(trade);
            update_upl();
            update_ui();
        }

        void update_ui()
        {
            dataGridView1.DataSource = trades_db;
            try
            {
                //dataGridView1.Columns["id"].DataPropertyName = "id";
                dataGridView1.Columns["selected"].DataPropertyName = "selected";
                dataGridView1.Columns["selected"].SortMode = DataGridViewColumnSortMode.Automatic;

                dataGridView1.Columns["pair"].DataPropertyName = "pair";
                dataGridView1.Columns["pair"].SortMode = DataGridViewColumnSortMode.Automatic;

                dataGridView1.Columns["sl"].DataPropertyName = "sl";
                dataGridView1.Columns["sl"].SortMode = DataGridViewColumnSortMode.Automatic;

                dataGridView1.Columns["tp"].DataPropertyName = "tp";
                dataGridView1.Columns["tp"].SortMode = DataGridViewColumnSortMode.Automatic;               


                dataGridView1.Columns["lots"].DataPropertyName = "lots";
                dataGridView1.Columns["lots"].SortMode = DataGridViewColumnSortMode.Automatic;

                dataGridView1.Columns["upl"].DataPropertyName = "upl";
                dataGridView1.Columns["upl"].SortMode = DataGridViewColumnSortMode.Automatic;
            }
            catch { }

            dataGridView1.Invoke((MethodInvoker)(() => dataGridView1.Refresh()));
            //dataGridView1.Refresh();

            string text = null;
            foreach (Trade trade in trades_db)
            {
                text += trade.selected + " ";
                text += trade.ID + " ";
                text += trade.pair + " ";
                text += trade.sl + " ";
                text += trade.tp + " ";
                text += trade.lots + " ";
                text += trade.upl + " ";
                text += Environment.NewLine;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if(Convert.ToDouble(row.Cells["upl"].Value) > 0)
                {
                    
                    DataGridViewCell cell = dataGridView1.Rows[row.Index].Cells[dataGridView1.Columns["upl"].Index];


                    DataGridViewButtonColumn c = (DataGridViewButtonColumn)dataGridView1.Columns["upl"];
                    dataGridView1.Rows[row.Index].Cells[dataGridView1.Columns["upl"].Index].Style.BackColor = Color.LimeGreen;
                    c.FlatStyle = FlatStyle.Popup;
                    c.DefaultCellStyle.ForeColor = Color.Black;
                    c.DefaultCellStyle.BackColor = Color.LimeGreen;
                }
                else
                {
                    //DataGridViewButtonColumn c = (DataGridViewButtonColumn)dataGridView1.Columns["upl"];
                    //c.FlatStyle = FlatStyle.Popup;
                    //c.DefaultCellStyle.ForeColor = Color.Black;
                    //c.DefaultCellStyle.BackColor = Color.Red;
                    dataGridView1.Rows[row.Index].Cells[dataGridView1.Columns["upl"].Index].Style.BackColor = Color.Red;
                }
            }

            buy_button.Invoke((MethodInvoker)(() => buy_button.Text = ask.ToString()));
            textBox1.Text = text;
        }

        void update_upl()
        {
            Random rand = new Random();
            ask = Convert.ToDecimal(rand.NextDouble()) + 1;
            foreach (Trade trade in trades_db)
            {
                int selector = rand.Next(200);              

                double profit = rand.NextDouble();
                if(selector > 100) profit *= -1;
                trade.upl = decimal.Round(Convert.ToDecimal(profit), 2, MidpointRounding.AwayFromZero);                             
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // numericUpDown1.Value = Convert.ToDecimal(listBox1.SelectedItem);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buy();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            close_order();
        }

        void close_order()
        {
            //get trade id from the datasource
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];


                int id = Convert.ToInt32(selectedRow.Cells["trade_id"].Value);


                for (int i = 0; i < trades_db.Count; i++)
                {
                    Trade trade = trades_db[i];
                    if(trade.ID == id)
                    {
                        trades_db.RemoveAt(i);
                        break;
                    }
                }
            }

            dataGridView1.Refresh();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.SelectedCells.Count > 0)
            //{
            //    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            //    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            //    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);

            //    for (int i = 0; i < trades_db.Count; i++)
            //    {
            //        Trade trade = trades_db[i];
            //        if (trade.ID == id)
            //        {
            //            trades_db.RemoveAt(i);
            //            break;
            //        }
            //    }
            //}

            //dataGridView1.Refresh();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                close_order();
            }
        }

        void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells[0].ColumnIndex == 0) dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}

class Trade
{
    public bool selected { get; set; }
    public int ID { get; }
    public string pair { get; }
    public decimal sl { get; set; }
    public decimal tp { get; set; }
    public decimal lots { get; set; }
    public decimal upl { get; set; }    

    public Trade(int id, string pair, decimal sl, decimal tp, decimal lots, decimal upl, bool selected = false)
    {
        this.selected = selected;
        this.pair = pair;
        this.ID = id;
        this.sl = sl;
        this.tp = tp;
        this.lots = lots;
        this.upl = upl;
    }
}
public class SortableBindingList<T> : BindingList<T> where T : class
{
    private bool _isSorted;
    private ListSortDirection _sortDirection = ListSortDirection.Ascending;
    private PropertyDescriptor _sortProperty;

    /// <summary>
    /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
    /// </summary>
    public SortableBindingList()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
    /// </summary>
    /// <param name="list">An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the <see cref="T:System.ComponentModel.BindingList`1" />.</param>
    public SortableBindingList(IList<T> list)
        : base(list)
    {
    }

    /// <summary>
    /// Gets a value indicating whether the list supports sorting.
    /// </summary>
    protected override bool SupportsSortingCore
    {
        get { return true; }
    }

    /// <summary>
    /// Gets a value indicating whether the list is sorted.
    /// </summary>
    protected override bool IsSortedCore
    {
        get { return _isSorted; }
    }

    /// <summary>
    /// Gets the direction the list is sorted.
    /// </summary>
    protected override ListSortDirection SortDirectionCore
    {
        get { return _sortDirection; }
    }

    /// <summary>
    /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null
    /// </summary>
    protected override PropertyDescriptor SortPropertyCore
    {
        get { return _sortProperty; }
    }

    /// <summary>
    /// Removes any sort applied with ApplySortCore if sorting is implemented
    /// </summary>
    protected override void RemoveSortCore()
    {
        _sortDirection = ListSortDirection.Ascending;
        _sortProperty = null;
        _isSorted = false; //thanks Luca
    }

    /// <summary>
    /// Sorts the items if overridden in a derived class
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="direction"></param>
    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
        _sortProperty = prop;
        _sortDirection = direction;

        List<T> list = Items as List<T>;
        if (list == null) return;

        list.Sort(Compare);

        _isSorted = true;
        //fire an event that the list has been changed.
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }


    private int Compare(T lhs, T rhs)
    {
        var result = OnComparison(lhs, rhs);
        //invert if descending
        if (_sortDirection == ListSortDirection.Descending)
            result = -result;
        return result;
    }

    private int OnComparison(T lhs, T rhs)
    {
        object lhsValue = lhs == null ? null : _sortProperty.GetValue(lhs);
        object rhsValue = rhs == null ? null : _sortProperty.GetValue(rhs);
        if (lhsValue == null)
        {
            return (rhsValue == null) ? 0 : -1; //nulls are equal
        }
        if (rhsValue == null)
        {
            return 1; //first has value, second doesn't
        }
        if (lhsValue is IComparable)
        {
            return ((IComparable)lhsValue).CompareTo(rhsValue);
        }
        if (lhsValue.Equals(rhsValue))
        {
            return 0; //both are the same
        }
        //not comparable, compare ToString
        return lhsValue.ToString().CompareTo(rhsValue.ToString());
    }
}