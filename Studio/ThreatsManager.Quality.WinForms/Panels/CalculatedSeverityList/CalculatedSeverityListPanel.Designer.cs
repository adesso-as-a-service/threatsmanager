﻿using System.Linq;
using DevComponents.DotNetBar.Layout;
using DevComponents.DotNetBar.SuperGrid;
using ThreatsManager.Utilities;
using ItemEditor = ThreatsManager.Utilities.WinForms.ItemEditor;

namespace ThreatsManager.Quality.Panels.CalculatedSeverityList
{
    partial class CalculatedSeverityListPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            _grid.CellActivated -= _grid_CellActivated;
            _grid.CellMouseDown -= _grid_CellMouseDown;
            _grid.CellMouseLeave -= _grid_CellMouseLeave;
            _grid.CellMouseMove -= _grid_CellMouseMove;
            _grid.RowActivated -= _grid_RowActivated;
            _grid.MouseClick -= _grid_MouseClick;

            _properties.OpenDiagram -= OpenDiagram;
            _properties.Item = null;

            GridTextBoxDropDownEditControl ddc = _grid.PrimaryGrid.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
            if (ddc != null)
            {
                ddc.ButtonClearClick -= DdcButtonClearClick;
            }

            if (_model != null)
            {
                _model.ChildRemoved -= ModelChildRemoved;
                _model.ThreatEventAdded -= OnThreatEventAdded;
                _model.ThreatEventRemoved -= OnThreatEventRemoved;
                _model.ThreatEventAddedToEntity -= OnThreatEventAdded;
                _model.ThreatEventRemovedFromEntity -= OnThreatEventRemoved;
                _model.ThreatEventAddedToDataFlow -= OnThreatEventAdded;
                _model.ThreatEventRemovedFromDataFlow -= OnThreatEventRemoved;
            }

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    RemoveEventSubscriptions(row);
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._properties = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this._topLeftPanel = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._apply = new System.Windows.Forms.Button();
            this._filter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this._tooltipTimer = new System.Windows.Forms.Timer(this.components);
            this._topLeftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _properties
            // 
            this._properties.BackColor = System.Drawing.Color.White;
            this._properties.Dock = System.Windows.Forms.DockStyle.Right;
            this._properties.Item = null;
            this._properties.Location = new System.Drawing.Point(525, 0);
            this._properties.Name = "_properties";
            this._properties.ReadOnly = false;
            this._properties.Size = new System.Drawing.Size(309, 512);
            this._properties.TabIndex = 0;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.expandableSplitter1.ExpandableControl = this._properties;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.ForeColor = System.Drawing.Color.Black;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(519, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(6, 512);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 1;
            this.expandableSplitter1.TabStop = false;
            // 
            // _topLeftPanel
            // 
            this._topLeftPanel.BackColor = System.Drawing.Color.White;
            this._topLeftPanel.Controls.Add(this._apply);
            this._topLeftPanel.Controls.Add(this._filter);
            this._topLeftPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topLeftPanel.ForeColor = System.Drawing.Color.Black;
            this._topLeftPanel.Location = new System.Drawing.Point(0, 0);
            this._topLeftPanel.Name = "_topLeftPanel";
            // 
            // 
            // 
            this._topLeftPanel.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this._topLeftPanel.Size = new System.Drawing.Size(519, 36);
            this._topLeftPanel.TabIndex = 2;
            // 
            // _apply
            // 
            this._apply.Location = new System.Drawing.Point(440, 4);
            this._apply.Margin = new System.Windows.Forms.Padding(0);
            this._apply.Name = "_apply";
            this._apply.Size = new System.Drawing.Size(75, 23);
            this._apply.TabIndex = 1;
            this._apply.Text = "Apply";
            this._apply.UseVisualStyleBackColor = true;
            this._apply.Click += new System.EventHandler(this._apply_Click);
            // 
            // _filter
            // 
            this._filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._filter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._filter.Border.Class = "TextBoxBorder";
            this._filter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._filter.ButtonCustom.Symbol = "";
            this._filter.ButtonCustom.Visible = true;
            this._filter.DisabledBackColor = System.Drawing.Color.White;
            this._filter.ForeColor = System.Drawing.Color.Black;
            this._filter.Location = new System.Drawing.Point(36, 4);
            this._filter.Margin = new System.Windows.Forms.Padding(0);
            this._filter.Name = "_filter";
            this._filter.PreventEnterBeep = true;
            this._filter.Size = new System.Drawing.Size(396, 20);
            this._filter.TabIndex = 0;
            this._filter.ButtonCustomClick += new System.EventHandler(this._filter_ButtonCustomClick);
            this._filter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._filter_KeyPress);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._filter;
            this.layoutControlItem1.Height = 31;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Filter";
            this.layoutControlItem1.Width = 99;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._apply;
            this.layoutControlItem2.Height = 31;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 83;
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(0, 36);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(519, 476);
            this._grid.TabIndex = 3;
            this._grid.Text = "superGridControl1";
            this._grid.CellActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellActivatedEventArgs>(this._grid_CellActivated);
            this._grid.CellMouseDown += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this._grid_CellMouseDown);
            this._grid.CellMouseLeave += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellEventArgs>(this._grid_CellMouseLeave);
            this._grid.CellMouseMove += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this._grid_CellMouseMove);
            this._grid.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this._grid_RowActivated);
            this._grid.MouseClick += new System.Windows.Forms.MouseEventHandler(this._grid_MouseClick);
            // 
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.DelayTooltipHideDuration = 250;
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._superTooltip.MarkupLinkClick += new DevComponents.DotNetBar.MarkupLinkClickEventHandler(this._superTooltip_MarkupLinkClick);
            // 
            // _tooltipTimer
            // 
            this._tooltipTimer.Interval = 1000;
            this._tooltipTimer.Tick += new System.EventHandler(this._tooltipTimer_Tick);
            // 
            // CalculatedSeverityListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._topLeftPanel);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this._properties);
            this.Name = "CalculatedSeverityListPanel";
            this.Size = new System.Drawing.Size(834, 512);
            this._topLeftPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ItemEditor _properties;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.Layout.LayoutControl _topLeftPanel;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private System.Windows.Forms.Button _apply;
        private DevComponents.DotNetBar.Controls.TextBoxX _filter;
        private LayoutControlItem layoutControlItem1;
        private LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
        private System.Windows.Forms.Timer _tooltipTimer;
    }
}
