using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.LevelSystems {

	class LevelSystem {

		internal static async Task Main(MessageCreateEventArgs Message_Objects) {
			DiscordMessage NextMessage = await Message_Objects.Message.AwaitMessage(1 * 60 * 1000);
			if (NextMessage != null && NextMessage.Author.IsBot) return;

			string MessageID = Message_Objects.Message.Id.ToString();
			string MessageContentSource = Message_Objects.Message.Content;
			ulong UserID = Message_Objects.Message.Author.Id;

			(int MessageCount, string MessageIDReplace) = ExpCalculator.ExpConvert(MessageID, MessageContentSource);
			double Exp = ExpCalculator.ExpCalculate(MessageCount, MessageIDReplace);

			Exp += await DatabaseMethods.ExpFind(UserID).ConfigureAwait(false);
			uint BeforeLevel = await DatabaseMethods.LevelFind(UserID).ConfigureAwait(false);
			uint AfterLevel = LevelDecision(Exp, BeforeLevel);
			Log.Debug("BeforeLevel: " + BeforeLevel);

			await DatabaseMethods.DataUpsert(UserID, AfterLevel, Exp).ConfigureAwait(false);

			if (BeforeLevel < AfterLevel) {
				ulong LogChannelID = await DatabaseMethods.LogChannelFind(Message_Objects.Guild.Id).ConfigureAwait(false);
				if (LogChannelID == 0) {
					Log.Debug("LogChannel Not Found");
					return;
				}
				else {
					Log.Debug("Send");
					try {
						DiscordChannel LogChannel = Message_Objects.Guild.GetChannel(LogChannelID);
						DiscordEmbed LevelUpEmbed = new DiscordEmbedBuilder()
						.WithTitle("レベルが上がりました！")
						.WithDescription(string.Format("経験値: {0}\nレベル: Lv.{1} -> Lv.{2}", Exp, BeforeLevel, AfterLevel))
						.WithColor(new DiscordColor(0xFFFF00))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", Message_Objects.Client.CurrentUser.Username));

						await LogChannel.SendMessageAsync(default, default, LevelUpEmbed);
					}
					catch (Exception Error) {
						Log.Error(Error);
					}
				}
			}
			else {
				Log.Debug("Not Send");
			}
		}

		static uint LevelDecision(double Exp, uint BeforeLevel) {
			double LevelReqExp = ReqNextLevelExp(BeforeLevel);
			Log.Debug("LevelReqExp: " + LevelReqExp);

			if (Exp >= LevelReqExp) {
				Log.Debug("AfterLevel: " + (BeforeLevel + 1));
				return BeforeLevel + 1;
			}
			else {
				Log.Debug("AfterLevel: " + BeforeLevel);
				return BeforeLevel;
			}
		}

		internal static double ReqNextLevelExp(uint BeforeLevel) =>
			(Math.Pow((int) BeforeLevel - 2, 2) * 10.0) + (1000.0 * (BeforeLevel - 1))
			+ (Math.Pow(BeforeLevel - 1, 2) * 10.0) + (1000.0 * BeforeLevel);
	}
}
