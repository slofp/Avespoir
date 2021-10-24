using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using Avespoir.Core.Database.DatabaseMethods;

namespace Avespoir.Core.Modules.LevelSystems {

	class LevelSystem {

		internal static async Task Main(MessageObject Message_Object) {
			Log.Debug("Level System Start");
			StackMessage Stack_Messages = await new StackMessage(Message_Object).Start().ConfigureAwait(false);

			if (!Stack_Messages.AllowExp) {
				Log.Debug("Exp Not get");
				return;
			}

			double Exp = 0.00;
			ulong UserID = Stack_Messages.StackMessages[0].Author.Id;
			ulong EndUserID = Stack_Messages.StackMessages[^1].Author.Id;
			Log.Debug("Sender: " + UserID);
			Log.Debug("EndSender: " + EndUserID);
			foreach (DiscordMessage Stack_Message in Stack_Messages.StackMessages) {
				string MessageID = Stack_Message.Id.ToString();
				string MessageContentSource = Stack_Message.Content;

				(int MessageCount, string MessageIDReplace) = ExpCalculator.ExpConvert(MessageID, MessageContentSource);
				Exp += ExpCalculator.ExpCalculate(MessageCount, MessageIDReplace);
			}

			await SendDB(UserID, Exp, Message_Object).ConfigureAwait(false);
			await SendDB(EndUserID, Exp, Message_Object).ConfigureAwait(false);
		}

		static async Task SendDB(ulong UserID, double Exp, MessageObject Message_Object) {
			Exp += UserDataMethods.ExpFind(UserID);
			uint BeforeLevel = UserDataMethods.LevelFind(UserID);
			uint AfterLevel = LevelDecision(Exp, BeforeLevel);
			Log.Debug("BeforeLevel: " + BeforeLevel);

			UserDataMethods.DataUpsert(UserID, AfterLevel, Exp);

			if (BeforeLevel < AfterLevel) {
				ulong LogChannelID = GuildConfigMethods.LogChannelFind(Message_Object.Guild.Id);
				if (LogChannelID == 0) {
					Log.Debug("LogChannel Not Found");
					return;
				}
				else {
					Log.Debug("Send");
					try {
						GetLanguage Get_Language = new GetLanguage(GuildConfigMethods.LanguageFind(Message_Object.Guild.Id));

						DiscordChannel LogChannel = Message_Object.Guild.GetChannel(LogChannelID);
						DiscordMember Member = await Message_Object.Guild.GetMemberAsync(UserID).ConfigureAwait(false);
						DiscordEmbedBuilder LevelUpEmbed = new DiscordEmbedBuilder()
						.WithAuthor(Member.Username + "#" + Member.Discriminator, default, Member.GetAvatarUrl(ImageFormat.Auto, 1024))
						.WithTitle(Get_Language.Language_Data.LevelUpEmbed1)
						.WithDescription(string.Format(Get_Language.Language_Data.LevelUpEmbed2, Exp, BeforeLevel, AfterLevel))
						.WithColor(new DiscordColor(0xFFFF00))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));

						await LogChannel.SendMessageAsync(embed: LevelUpEmbed.Build());
					}
					catch (Exception Error) {
						Log.Error("", Error);
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
