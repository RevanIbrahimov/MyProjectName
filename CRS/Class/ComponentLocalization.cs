using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars.Localization;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraTreeList.Localization;
using DevExpress.XtraScheduler.Localization;

namespace CRS.Class
{
    class ComponentLocalization
    {
        public class CustomGridLocalizer : GridLocalizer
        {
            public override string GetLocalizedString(GridStringId id)
            {
                if (id == GridStringId.FilterBuilderApplyButton)
                    return "Tətbiq et";
                if (id == GridStringId.FilterBuilderCancelButton)
                    return "Bağla";
                if (id == GridStringId.FilterBuilderCaption)
                    return "Filtrlərin yaradılması";
                if (id == GridStringId.FilterPanelCustomizeButton)
                    return "Filtri dəyiş";
                if (id == GridStringId.MenuColumnBestFit)
                    return "Seçilmiş sütunun enini cədvələ görə uyğunlaşdır";
                if (id == GridStringId.MenuColumnBestFitAllColumns)
                    return "Bütün sütunların enini cədvələ görə uyğunlaşdır";
                if (id == GridStringId.MenuColumnClearAllSorting)
                    return "Bütün sıralamaları sil";
                if (id == GridStringId.MenuColumnClearFilter)
                    return "Filtri sil";
                if (id == GridStringId.MenuColumnClearSorting)
                    return "Sıralamanı sil";
                if (id == GridStringId.MenuColumnFilterEditor)
                    return "Filtrlər";
                if (id == GridStringId.MenuColumnSortAscending)
                    return "Artan sırada düz";
                if (id == GridStringId.MenuColumnSortDescending)
                    return "Azalan sırada düz";
                if (id == GridStringId.MenuColumnFindFilterShow)
                    return "Axtarış panelini göstər";
                if (id == GridStringId.MenuColumnFindFilterHide)
                    return "Axtarış panelini gizlət";
                if (id == GridStringId.MenuColumnAutoFilterRowShow)
                    return "Avtomatik axtarış panelini göstər";
                if (id == GridStringId.MenuColumnAutoFilterRowHide)
                    return "Avtomatik axtarış panelini gizlət";
                if (id == GridStringId.MenuColumnRemoveColumn)
                    return "Sütunu gizlət";
                if (id == GridStringId.MenuColumnShowColumn)
                    return "Sütunu göstər";
                if (id == GridStringId.MenuColumnGroup)
                    return "Sütunu qruplaşdır";
                if (id == GridStringId.MenuColumnUnGroup)
                    return "Sütunu qrupdan çıxart";
                if (id == GridStringId.MenuGroupPanelShow)
                    return "Qrup panelini göstər";
                if (id == GridStringId.MenuGroupPanelHide)
                    return "Qrup panelini gizlət";
                if (id == GridStringId.MenuGroupPanelClearGrouping)
                    return "Bütün sütunları qruplaşmadan sil";
                if (id == GridStringId.MenuGroupPanelFullCollapse)
                    return "Qruplaşmış sütunu bağla";
                if (id == GridStringId.MenuGroupPanelFullExpand)
                    return "Qruplaşmış sütunu aç";
                if (id == GridStringId.MenuColumnColumnCustomization)
                    return "Gizlədilmiş sütunlar";
                if (id == GridStringId.CheckboxSelectorColumnCaption)
                    return "Bütün sətirləri seç";
                if (id == GridStringId.FindNullPrompt)
                    return "Açar sözü daxil edin";
                if (id == GridStringId.FindControlClearButton)
                    return "Təmizlə";
                if (id == GridStringId.FindControlFindButton)
                    return "Axtar";
                if (id == GridStringId.GridGroupPanelText)
                    return "Qruplaşdırmaq istədiyiniz sütunu buraya sürükləyib buraxın";
                if (id == GridStringId.CustomizationCaption)
                    return "Gizlədilmiş sütunlar";
                if (id == GridStringId.MenuFooterSum)
                    return "Cəm";
                if (id == GridStringId.MenuFooterSumFormat)
                    return "Cəm = {0}";
                if (id == GridStringId.MenuFooterMax)
                    return "Ən böyük";
                if (id == GridStringId.MenuFooterMaxFormat)
                    return "Ən böyük = {0}";
                if (id == GridStringId.MenuFooterMin)
                    return "Ən kiçik";
                if (id == GridStringId.MenuFooterMinFormat)
                    return "Ən kiçik = {0}";
                if (id == GridStringId.MenuFooterAverage)
                    return "Ədədi orta";
                if (id == GridStringId.MenuFooterAverageFormat)
                    return "Ədədi orta = {0}";
                if (id == GridStringId.MenuFooterNone)
                    return "Hesablama yoxdur";
                if (id == GridStringId.MenuFooterCount)
                    return "Say";
                if (id == GridStringId.MenuFooterCountFormat)
                    return "Say = {0}";
                if (id == GridStringId.MenuFooterAddSummaryItem)
                    return "Digər hesablama əlavə et";
                if (id == GridStringId.MenuFooterClearSummaryItems)
                    return "Hesablamaları sil";
                if (id == GridStringId.PopupFilterCustom)
                    return "(Xüsusi filtr)";
                if (id == GridStringId.PopupFilterBlanks)
                    return "(Boş olanlar)";
                if (id == GridStringId.PopupFilterNonBlanks)
                    return "(Boş olmayanlar)";
                if (id == GridStringId.PopupFilterAll)
                    return "Hamısı";
                if (id == GridStringId.CustomFilterDialogFormCaption)
                    return "Xüsusi filtrlər";
                if (id == GridStringId.CustomFilterDialogEmptyValue)
                    return "dəyəri daxil et";
                if (id == GridStringId.CustomFilterDialogRadioAnd)
                    return "Və";
                if (id == GridStringId.CustomFilterDialogRadioOr)
                    return "Və ya";
                if (id == GridStringId.CustomFilterDialogCancelButton)
                    return "İmtina et";
                if (id == GridStringId.CustomFilterDialogOkButton)
                    return "Təsdiq et";
                if (id == GridStringId.CustomFilterDialogCaption)
                    return "Şərt sətirində göstər";
                if (id == GridStringId.EditFormUpdateButton)
                    return "Dəyiş";
                if (id == GridStringId.EditFormCancelButton)
                    return "Bağla";
                return base.GetLocalizedString(id);
            }
        }

        public class StringLocalizer : Localizer
        {
            public override string GetLocalizedString(StringId id)
            {
                if (id == StringId.XtraMessageBoxYesButtonText)
                    return "Bəli";
                if (id == StringId.XtraMessageBoxNoButtonText)
                    return "Xeyr";
                if (id == StringId.XtraMessageBoxCancelButtonText)
                    return "Bağla";
                if (id == StringId.FilterAggregateAvg)
                    return "Ədədi orta";
                if (id == StringId.FilterShowAll)
                    return "Hamısını seç";
                if (id == StringId.Cancel)
                    return "Bağla";
                if (id == StringId.OK)
                    return "Tətbiq et";
                if (id == StringId.DateEditClear)
                    return "Təmizlə";
                if (id == StringId.DateEditToday)
                    return "Bu gün";
                if (id == StringId.TextEditMenuUndo)
                    return "Geri qaytar";
                if (id == StringId.TextEditMenuSelectAll)
                    return "Hamısını seç";
                if (id == StringId.TextEditMenuPaste)
                    return "Yapışdır";
                if (id == StringId.TextEditMenuDelete)
                    return "Sil";
                if (id == StringId.TextEditMenuCut)
                    return "Kəs";
                if (id == StringId.TextEditMenuCopy)
                    return "Surətini çıxar";
                if(id == StringId.PictureEditMenuCopy)
                    return "Surətini çıxar";
                if (id == StringId.PictureEditMenuDelete)
                    return "Sil";
                if (id == StringId.PictureEditMenuCut)
                    return "Kəs";
                if (id == StringId.PictureEditMenuPaste)
                    return "Yapışdır";
                if (id == StringId.PictureEditMenuSave)
                    return "Yadda saxla";
                if (id == StringId.PictureEditMenuLoad)
                    return "Yüklə";
                if (id == StringId.InvalidValueText)
                    return "Format düz deyil";
                return base.GetLocalizedString(id);
            }
        }

        public class CustomBarLocalizer : BarLocalizer
        {
            public override string GetLocalizedString(BarString id)
            {
                if (id == BarString.SkinsBonus)
                    return "Bonus görünüşlər";
                if (id == BarString.SkinsOffice)
                    return "Office görünüşlər";
                if (id == BarString.SkinsTheme)
                    return "Mövzu gürünüşlər";
                if (id == BarString.SkinsMain)
                    return "Standart gürünüşlər";
                if (id == BarString.SkinCaptions)
                    return "Üzlüklər";
                if (id == BarString.RibbonToolbarMinimizeRibbon)
                    return "Paneli gizlət (Ctrl + F1)";               
                return base.GetLocalizedString(id);
            }
        }

        public class CustomDockManagerLocalizer : DockManagerResXLocalizer
        {
            public override string GetLocalizedString(DockManagerStringId id)
            {
                if (id == DockManagerStringId.CommandClose)
                    return "Bağla";
                if (id == DockManagerStringId.CommandAutoHide)
                    return "Gizlət";
                if (id == DockManagerStringId.CommandDock)
                    return "İlkin vəziyyəti";
                if (id == DockManagerStringId.CommandFloat)
                    return "Yer dəyişdir";
                if (id == DockManagerStringId.CommandMaximize)
                    return "Tam ölçü";
                if (id == DockManagerStringId.CommandRestore)
                    return "Geri qaytar";
                return base.GetLocalizedString(id);
            }
        }

        public class CustomTreeListLocalizer : TreeListLocalizer
        {
            public override string GetLocalizedString(TreeListStringId id)
            {
                if (id == TreeListStringId.MenuColumnBestFit)
                    return "Seçilmiş sütunun enini cədvələ görə uyğunlaşdır";
                if (id == TreeListStringId.MenuColumnBestFitAllColumns)
                    return "Bütün sütunların enini cədvələ görə uyğunlaşdır";
                if (id == TreeListStringId.MenuColumnSortAscending)
                    return "Artan sırada düz";
                if (id == TreeListStringId.MenuColumnSortDescending)
                    return "Azalan sırada düz";
                if (id == TreeListStringId.MenuColumnClearSorting)
                    return "Sıralamanı sil";
                if (id == TreeListStringId.MenuColumnColumnCustomization)
                    return "Gizlədilmiş sütunlar";
                if (id == TreeListStringId.ColumnCustomizationText)
                    return "Gizlədilmiş sütunlar";
                if (id == TreeListStringId.CustomizationFormBandHint)
                    return "Gizlədilmiş";
                if (id == TreeListStringId.MenuColumnFindFilterShow)
                    return "Axtarış panelini göstər";
                if (id == TreeListStringId.MenuColumnAutoFilterRowShow)
                    return "Avtomatik axtarış panelini göstər";
                if (id == TreeListStringId.MenuColumnFindFilterHide)
                    return "Axtarış panelini gizlət";
                if (id == TreeListStringId.MenuColumnAutoFilterRowHide)
                    return "Avtomatik axtarış panelini gizlət";
                return base.GetLocalizedString(id);
            }
        }

        public class CustomerSchedulerLocalizer : SchedulerLocalizer
        {
            public override string GetLocalizedString(SchedulerStringId id)
            {
                switch (id)
                {                    
                    case SchedulerStringId.MenuCmd_NavigateBackward:
                        return "Geri";
                    case SchedulerStringId.MenuCmd_NavigateForward:
                        return "İrəli";
                    case SchedulerStringId.MenuCmd_GotoToday:
                        return "Bu gün";
                    case SchedulerStringId.MenuCmd_Print:
                        return "Çap et";
                    case SchedulerStringId.MenuCmd_PrintPreview:
                        return "Göstər";
                    case SchedulerStringId.MenuCmd_PrintPageSetup:
                        return "Səhifəni sazla";
                    case SchedulerStringId.MenuCmd_ViewZoomIn:
                        return "Həcmi böyüt";
                    case SchedulerStringId.MenuCmd_ViewZoomOut:
                        return "Həcmi kiçilt";
                    case SchedulerStringId.MenuCmd_10Minutes:
                        return "10 dəqiqə";
                    case SchedulerStringId.MenuCmd_15Minutes:
                        return "15 dəqiqə";
                    case SchedulerStringId.MenuCmd_20Minutes:
                        return "20 dəqiqə";
                    case SchedulerStringId.MenuCmd_30Minutes:
                        return "30 dəqiqə";
                    case SchedulerStringId.MenuCmd_60Minutes:
                        return "60 dəqiqə";
                    case SchedulerStringId.MenuCmd_5Minutes:
                        return "5 dəqiqə";
                    case SchedulerStringId.MenuCmd_6Minutes:
                        return "6 dəqiqə";
                    case SchedulerStringId.MenuCmd_SwitchToGroupByResource:
                        return "Məhkəmə ilə qruplaşma";
                    case SchedulerStringId.MenuCmd_SwitchToGroupByDate:
                        return "Tarix ilə qruplaşma";
                    case SchedulerStringId.MenuCmd_SwitchToGroupByNone:
                        return "Qruplaşdırma yoxdur";
                    case SchedulerStringId.MenuCmd_SwitchToDayView:
                        return "1 gün";
                    case SchedulerStringId.MenuCmd_SwitchToFullWeekView:
                        return "1 həftə";
                    case SchedulerStringId.MenuCmd_SwitchToMonthView:
                        return "1 ay";
                    case SchedulerStringId.MenuCmd_SwitchToWorkWeekView:
                        return "İş günləri";
                    case SchedulerStringId.MenuCmd_SwitchToTimelineView:
                        return "Saat";
                    case SchedulerStringId.MenuCmd_SwitchToGanttView:
                        return "Ümumi";
                    case SchedulerStringId.MenuCmd_TimeScalesMenu:
                        return "Vaxtın miqyası";
                    case SchedulerStringId.MenuCmd_TimeScaleCaptionsMenu:
                        return "Miqyasın adları";
                    case SchedulerStringId.MenuCmd_ChangeTimelineScaleWidth:
                        return "Miqyasın eni";
                    case SchedulerStringId.MenuCmd_TimeScaleHour:
                        return "Saat";
                    case SchedulerStringId.MenuCmd_TimeScaleMonth:
                        return "Ay";
                    case SchedulerStringId.MenuCmd_TimeScaleDay:
                        return "Gün";
                    case SchedulerStringId.MenuCmd_TimeScaleQuarter:
                        return "Rüb";
                    case SchedulerStringId.MenuCmd_TimeScaleYear:
                        return "İl";
                    case SchedulerStringId.MenuCmd_TimeScaleWeek:
                        return "Həftə";
                    case SchedulerStringId.MenuCmd_CompressWeekend:
                        return "Şənbə/bazar bir xanada";
                    case SchedulerStringId.MenuCmd_ShowWorkTimeOnly:
                        return "İş saatları";
                    case SchedulerStringId.MenuCmd_CellsAutoHeight:
                        return "Eni avtomatik dəyiş";
                    case SchedulerStringId.MenuCmd_GotoDate:
                        return "Günü seç";
                }
                return base.GetLocalizedString(id);
            }
        }
    }
}
