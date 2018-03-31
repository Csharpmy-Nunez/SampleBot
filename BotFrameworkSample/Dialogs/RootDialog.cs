using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using NetSpell;
using System.IO;
using System.Web.UI.WebControls;

namespace BotFrameworkSample.Dialogs
{
	[Serializable]
	public class RootDialog : IDialog<object>
	{
		public Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);

			return Task.CompletedTask;
		}

		private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
		{
			var activity = await result as Activity;
			NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
			string wordToCheck = activity.Text;

			try
			{

				
				oDict.DictionaryFile = "C:\\Program Files (x86)\\IIS Express\\en-US.dic";
				oDict.Initialize();
				NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling();
				oSpell.Dictionary = oDict;

			
				
				// calculate something for us to return
				int length = (activity.Text ?? string.Empty).Length;

				// return our reply to the user
				if (oSpell.TestWord(wordToCheck))
				{
					await context.PostAsync($"Word exists in the English dictionary");
				}
				else
				{
					await context.PostAsync($"Word does not exist in the English dictionary");
				}
			}
			catch (FileNotFoundException fnfe)
			{
				await context.PostAsync($"File Was Not Found | Details: {fnfe.Message}");
			}
			finally
			{
				oDict.Dispose();
			}

	
			//await context.PostAsync($"You sent {activity.Text} which was {length} characters From {activity.From}");

			context.Wait(MessageReceivedAsync);
		}
	}
}