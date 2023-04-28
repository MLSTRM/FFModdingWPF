﻿using Bartz24.FF13_2_LR;

namespace Bartz24.LR;

public class DataStoreBtAbility : DataStoreDB3SubEntry
{
    public int sStringResId_pointer { get; set; }
    public string sStringResId_string { get; set; }
    public int sInfoStResId_pointer { get; set; }
    public string sInfoStResId_string { get; set; }
    public int sScriptId_pointer { get; set; }
    public string sScriptId_string { get; set; }
    public int sAblArgStr0_pointer { get; set; }
    public string sAblArgStr0_string { get; set; }
    public int sAblArgStr1_pointer { get; set; }
    public string sAblArgStr1_string { get; set; }
    public int sAutoAblStEff0_pointer { get; set; }
    public string sAutoAblStEff0_string { get; set; }
    public int fDistanceMin { get; set; }
    public int fDistanceMax { get; set; }
    public int fMaxJumpHeight { get; set; }
    public int fYDistanceMin { get; set; }
    public int fYDistanceMax { get; set; }
    public int sReplaceAirAttack_pointer { get; set; }
    public string sReplaceAirAttack_string { get; set; }
    public int sReplaceAirAir_pointer { get; set; }
    public string sReplaceAirAir_string { get; set; }
    public int sReplaceFinish_pointer { get; set; }
    public string sReplaceFinish_string { get; set; }
    public int sReplaceEnAttr_pointer { get; set; }
    public string sReplaceEnAttr_string { get; set; }
    public int sActionId0_pointer { get; set; }
    public string sActionId0_string { get; set; }
    public int sActionId1_pointer { get; set; }
    public string sActionId1_string { get; set; }
    public int sActionId2_pointer { get; set; }
    public string sActionId2_string { get; set; }
    public int sActionId3_pointer { get; set; }
    public string sActionId3_string { get; set; }
    public int sRtDamSrc_pointer { get; set; }
    public string sRtDamSrc_string { get; set; }
    public int sRefDamSrc_pointer { get; set; }
    public string sRefDamSrc_string { get; set; }
    public int sSubRefDamSrc_pointer { get; set; }
    public string sSubRefDamSrc_string { get; set; }
    public int sSlamDamSrc_pointer { get; set; }
    public string sSlamDamSrc_string { get; set; }
    public int sCamArtsSeqId0_pointer { get; set; }
    public string sCamArtsSeqId0_string { get; set; }
    public int sCamArtsSeqId1_pointer { get; set; }
    public string sCamArtsSeqId1_string { get; set; }
    public int sCamArtsSeqId2_pointer { get; set; }
    public string sCamArtsSeqId2_string { get; set; }
    public int sCamArtsSeqId3_pointer { get; set; }
    public string sCamArtsSeqId3_string { get; set; }
    public int sRedirectAbility0_pointer { get; set; }
    public string sRedirectAbility0_string { get; set; }
    public int sRedirectTo0_pointer { get; set; }
    public string sRedirectTo0_string { get; set; }
    public int sRedirectAbility1_pointer { get; set; }
    public string sRedirectAbility1_string { get; set; }
    public int sRedirectTo1_pointer { get; set; }
    public string sRedirectTo1_string { get; set; }
    public int sRedirectAbility2_pointer { get; set; }
    public string sRedirectAbility2_string { get; set; }
    public int sRedirectTo2_pointer { get; set; }
    public string sRedirectTo2_string { get; set; }
    public int sRedirectAbility3_pointer { get; set; }
    public string sRedirectAbility3_string { get; set; }
    public int sRedirectTo3_pointer { get; set; }
    public string sRedirectTo3_string { get; set; }
    public int u1ComAbility { get; set; }
    public int u1RsvFlag0 { get; set; }
    public int u1RsvFlag1 { get; set; }
    public int u1RsvFlag2 { get; set; }
    public int u1RsvFlag3 { get; set; }
    public int u1RsvFlag4 { get; set; }
    public int u1RsvFlag5 { get; set; }
    public int u1RsvFlag6 { get; set; }
    public int u1RsvFlag7 { get; set; }
    public int u1OvKill { get; set; }
    public int u4ArtsNameHideKd { get; set; }
    public int u14ArtsNameFrame { get; set; }
    public int u4MenuCategory { get; set; }
    public int u8AblSndKind { get; set; }
    public int i12MenuSortNo { get; set; }
    public int u4TargetListKind { get; set; }
    public int u8AbilityKind { get; set; }
    public int i16ScriptArg0 { get; set; }
    public int i16ScriptArg1 { get; set; }
    public int i16AblArgInt0 { get; set; }
    public int i16AblArgInt1 { get; set; }
    public int u4UpAblKind { get; set; }
    public int i17AtbCount { get; set; }
    public int u1AtbOverHeat { get; set; }
    public int u4Lv { get; set; }
    public int u4AblType { get; set; }
    public int u1NoDespel { get; set; }
    public int u1TgPain { get; set; }
    public int i12AtRnd { get; set; }
    public int i12AtbStayCount { get; set; }
    public int u1TgFoge { get; set; }
    public int u1NoBackStep { get; set; }
    public int u1AIWanderFlag { get; set; }
    public int u1AutoAblStEfEd0 { get; set; }
    public int u4YRgCheckType { get; set; }
    public int u16TechInputFrame { get; set; }
    public int u16TechInputPrm0 { get; set; }
    public int u10TechBonusRate { get; set; }
    public int i16KeepVal { get; set; }
    public int u4AtDistKind { get; set; }
    public int u1CheckAutoRpl { get; set; }
    public int u1SeqParts { get; set; }
    public int u14TgElemId { get; set; }
    public int u10OpProp0 { get; set; }
    public int u8ExecAngle { get; set; }
    public int i16AutoAblStEfTi0 { get; set; }
    public int u4JumpAttackType { get; set; }
    public int u4ReplaceFinishExecCt { get; set; }
    public int u1SeqTermination { get; set; }
    public int u5ActSelType { get; set; }
    public int u10ReplaceCtg1 { get; set; }
    public int u10ReplaceCtg2 { get; set; }
    public int u10ReplaceCtg3 { get; set; }
    public int u8ShortCutNo { get; set; }
    public int u4LoopFinCond { get; set; }
    public int u16LoopFinArg { get; set; }
    public int i16RefDamSrcRpt { get; set; }
    public int i16SubRefDamSrcRp { get; set; }
    public int u8CamArtsSelType { get; set; }
}
