using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using System.Collections.Generic;

namespace Avespoir.Core.Modules.Utils {

	internal enum AwaitMessageStatus {
		Pending,
		Success
	}

	/// <summary>
	/// メモ: Messageを待機するときに使う
	/// MessageEventにstaticでDictionary<ulong, AwaitMessageInfo>で使いたい
	/// </summary>
	class AwaitMessageInfo {

		internal AwaitMessageStatus Status { get; set; }

		internal ulong MessageID { get; set; }

		internal ulong UserID { get; }

		/// <summary>
		/// Allowed All Users
		/// </summary>
		internal AwaitMessageInfo() {
			Status = AwaitMessageStatus.Pending;
			this.UserID = 0;
		}

		internal AwaitMessageInfo(ulong UserID) {
			Status = AwaitMessageStatus.Pending;
			this.UserID = UserID;
		}

		internal void Init(DiscordMessage Discord_Message) {
			if (!MessageEvent.AwaitMessageInfo_List_Dict.ContainsKey(Discord_Message.Channel.Id))
				MessageEvent.AwaitMessageInfo_List_Dict.Add(Discord_Message.Channel.Id, new List<AwaitMessageInfo>());

			MessageEvent.AwaitMessageInfo_List_Dict[Discord_Message.Channel.Id].Add(this);
		}

		// NOT DESTRUCTOR
		internal void Finalize_(DiscordMessage Discord_Message) {
			if (MessageEvent.AwaitMessageInfo_List_Dict.TryGetValue(Discord_Message.Channel.Id, out List<AwaitMessageInfo> AwaitMessageInfo_List)) {
				if (!AwaitMessageInfo_List.Remove(this)) Log.Warning("AwaitMessageInfo not Exist?????");
				if (AwaitMessageInfo_List.Count == 0) MessageEvent.AwaitMessageInfo_List_Dict.Remove(Discord_Message.Channel.Id);
			}
			else Log.Warning("AwaitMessageInfo List not Exist?????");
		}
	}
}
