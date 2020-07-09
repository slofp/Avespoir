using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.LevelSystems {

	class LevelSystem {

		internal static async Task Main(MessageCreateEventArgs Message_Objects) {
			Log.Debug("Level System Start");
			StackMessage Stack_Messages = new StackMessage(Message_Objects.Message);

			if (!Stack_Messages.AllowExp) {
				Log.Debug("Exp Not get");
				return;
			}

			double Exp = 0.00;
			ulong UserID = Message_Objects.Author.Id;
			foreach(DiscordMessage Stack_Message in Stack_Messages.StackDiscordMessages) {
				string MessageID = Stack_Message.Id.ToString();
				string MessageContentSource = Stack_Message.Content;

				(int MessageCount, string MessageIDReplace) = ExpCalculator.ExpConvert(MessageID, MessageContentSource);
				Exp += ExpCalculator.ExpCalculate(MessageCount, MessageIDReplace);
			}

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
						.WithAuthor(Message_Objects.Message.Author.Username + "#" + Message_Objects.Message.Author.Discriminator, default, Message_Objects.Message.Author.AvatarUrl)
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
			Log.Debug("Level System End");
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
