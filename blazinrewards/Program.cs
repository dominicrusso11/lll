﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace blazinrewards
{
	class Program
	{
		const string REGISTER_URL = "https://blazinrewards.com/Enrollment/Enrollment/ProfileRegistration";
		const string SURVEY_POST_URL = "https://blazinrewards.com/Profile/ProfileSurveys/SaveProfileSurvey";
		const string SURVEY_URL = "https://blazinrewards.com/Profile/ProfileSurveys/Survey";
		const string REDEEM_URL = "https://blazinrewards.com/Reward/Reward/RedeemReward";

		static void Main(string[] args)
		{
			using (HttpClient client = new HttpClient())
			{
				//Im a good programmer!!
				if (RegisterAccount(client)
					&& DoSurvey(client)
					&& GimmeCode(client))
				{
					Console.WriteLine("got dat code!!!");
				}
			}
		}

		private static bool GimmeCode(HttpClient client)
		{
			var form = new Dictionary<string, string>
				{
					{ "rewardCode", "96" },
				};

			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
			client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

			var content = new FormUrlEncodedContent(form);
			var postResp = client.PostAsync(REDEEM_URL, content).Result;
			dynamic postRespJson = JsonConvert.DeserializeObject(postResp.Content.ReadAsStringAsync().Result);

			if (postRespJson.type != "success")
			{
				Console.WriteLine($"Failed redeeming 2xp code: {postRespJson.message}");
				return false;
			}

			return true;
		}

		static bool DoSurvey(HttpClient client)
		{
			//We need this csrf token for the requests
			var resp = client.GetAsync(SURVEY_URL).Result.Content.ReadAsStringAsync().Result;

			var doc = new HtmlDocument();
			doc.LoadHtml(resp);

			var csrfToken = doc.DocumentNode.SelectNodes("//input[@name='__RequestVerificationToken']").FirstOrDefault().Attributes["value"].Value;

			//Disregard
			var content = WebUtility.UrlEncode("surveyResponseList[0][QuestionId]=33964564-c258-42ab-9380-bb77fe801046&surveyResponseList[0][ResponseTypeCode]=Checkbox&surveyResponseList[0][AnswersC][0][AnswerId]=a14f86bf-6f4b-4d0b-90ae-9603e1913f07&surveyResponseList[0][AnswersC][0][SurveyQuestionAnswerId]=&surveyResponseList[0][AnswersC][0][SurveyTextResponse]=&surveyResponseList[0][AnswersC][0][ProfileCheckListAnswers][0][answerId]=352fbaf3-9463-4a9a-b531-d0cd2a420783&surveyResponseList[0][AnswersC][0][ProfileCheckListAnswers][0][surveyQuestionAnswerId]=33964564-c258-42ab-9380-bb77fe801046352fbaf3-9463-4a9a-b531-d0cd2a420783&surveyResponseList[0][AnswersC][0][ProfileCheckListAnswers][1][answerId]=a14f86bf-6f4b-4d0b-90ae-9603e1913f07&surveyResponseList[0][AnswersC][0][ProfileCheckListAnswers][1][surveyQuestionAnswerId]=33964564-c258-42ab-9380-bb77fe801046a14f86bf-6f4b-4d0b-90ae-9603e1913f07&surveyResponseList[1][QuestionId]=c8d0429c-4b8d-4499-bc21-4391587d7335&surveyResponseList[1][ResponseTypeCode]=Checkbox&surveyResponseList[1][AnswersC][0][AnswerId]=ad27aafa-d688-4a70-a942-a12f714a9783&surveyResponseList[1][AnswersC][0][SurveyQuestionAnswerId]=&surveyResponseList[1][AnswersC][0][SurveyTextResponse]=&surveyResponseList[1][AnswersC][0][ProfileCheckListAnswers][0][answerId]=ad27aafa-d688-4a70-a942-a12f714a9783&surveyResponseList[1][AnswersC][0][ProfileCheckListAnswers][0][surveyQuestionAnswerId]=c8d0429c-4b8d-4499-bc21-4391587d7335ad27aafa-d688-4a70-a942-a12f714a9783&surveyResponseList[2][QuestionId]=2676aaae-63b7-4cc4-907a-094d49a09d24&surveyResponseList[2][ResponseTypeCode]=Drop-down (multi)&surveyResponseList[2][AnswersC][0][AnswerId]=dd41e6a7-8348-4fee-b52b-761078052f2b&surveyResponseList[2][AnswersC][0][SurveyQuestionAnswerId]=&surveyResponseList[2][AnswersC][0][SurveyTextResponse]=&surveyResponseList[2][AnswersC][0][ProfileCheckListAnswers][0][answerId]=dd41e6a7-8348-4fee-b52b-761078052f2b&surveyResponseList[2][AnswersC][0][ProfileCheckListAnswers][0][surveyQuestionAnswerId]=2676aaae-63b7-4cc4-907a-094d49a09d24dd41e6a7-8348-4fee-b52b-761078052f2b&surveyResponseList[3][QuestionId]=bca99055-6181-4e29-91b0-094f50e537ed&surveyResponseList[3][ResponseTypeCode]=Drop-down (multi)&surveyResponseList[3][AnswersC][0][AnswerId]=45f7a30d-0b88-440e-b08c-4a945d2a833e&surveyResponseList[3][AnswersC][0][SurveyQuestionAnswerId]=&surveyResponseList[3][AnswersC][0][SurveyTextResponse]=&surveyResponseList[3][AnswersC][0][ProfileCheckListAnswers][0][answerId]=45f7a30d-0b88-440e-b08c-4a945d2a833e&surveyResponseList[3][AnswersC][0][ProfileCheckListAnswers][0][surveyQuestionAnswerId]=bca99055-6181-4e29-91b0-094f50e537ed45f7a30d-0b88-440e-b08c-4a945d2a833e&surveyResponseList[4][QuestionId]=333d0f09-240b-46db-946c-a2170a3dbfe6&surveyResponseList[4][ResponseTypeCode]=Drop-down (multi)&surveyResponseList[4][AnswersC][0][AnswerId]=3867412c-2089-4fdc-b98a-c34b7b51c0be&surveyResponseList[4][AnswersC][0][SurveyQuestionAnswerId]=&surveyResponseList[4][AnswersC][0][SurveyTextResponse]=&surveyResponseList[4][AnswersC][0][ProfileCheckListAnswers][0][answerId]=3867412c-2089-4fdc-b98a-c34b7b51c0be&surveyResponseList[4][AnswersC][0][ProfileCheckListAnswers][0][surveyQuestionAnswerId]=333d0f09-240b-46db-946c-a2170a3dbfe63867412c-2089-4fdc-b98a-c34b7b51c0be&surveyId=ab959cbe-907c-4358-b941-d1a63cfe4a32&__RequestVerificationToken=" + csrfToken).Replace("%26", "&").Replace("%3D", "="); //Hack here at the end cuz C#


			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
			client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

			var postResp = client.PostAsync(SURVEY_POST_URL, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
			var data = postResp.Content.ReadAsStringAsync().Result;

			dynamic postRespJson = JsonConvert.DeserializeObject(postResp.Content.ReadAsStringAsync().Result);

			if (postRespJson.type != "success")
			{
				Console.WriteLine($"Failed saving survey: {postRespJson.message}");
				return false;
			}

			return true;
		}

		static bool RegisterAccount(HttpClient client)
		{
			//We need this csrf token for the requests
			var resp = client.GetAsync(REGISTER_URL).Result.Content.ReadAsStringAsync().Result;

			var doc = new HtmlDocument();
			doc.LoadHtml(resp);

			var csrfToken = doc.DocumentNode.SelectNodes("//input[@name='__RequestVerificationToken']").FirstOrDefault().Attributes["value"].Value;

			var email = GenerateEmail();

			var form = new Dictionary<string, string>
				{
					{ "__RequestVerificationToken", csrfToken },
					{ "FirstName", "Bob" },
					{ "LastName", "Smith" },
					{ "Address.PostalCode", "90002" },
					{"Phone.PhoneNumber", GeneratePhone() },
					{"Username", email },
					{"DOBMonth", "10" },
					{"DOBYear", "1980" },
					{"Password", "9yIoYwh5GOZu8ki" },
					{"ConfirmPassword", "9yIoYwh5GOZu8ki" },
					{"terms", "on" },
					{"hiddenzipcode", "" },
				};

			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
			client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

			var content = new FormUrlEncodedContent(form);
			var postResp = client.PostAsync(REGISTER_URL, content).Result;
			dynamic postRespJson = JsonConvert.DeserializeObject(postResp.Content.ReadAsStringAsync().Result);

			if (postRespJson.type != "success")
			{
				Console.WriteLine($"Failed making account: {postRespJson.message}");
				return false;
			}

			Console.WriteLine($"Credentials: Email: {email}, Password: 9yIoYwh5GOZu8ki");

			return true;
		}

		static string GeneratePhone()
		{
			var rand = new Random();

			return $"(213) {rand.Next(100, 999)}-{rand.Next(1000, 9999)}";
		}

		static string GenerateEmail(string domain = "zhorachu.com")
		{
			return $"{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8)}@{domain}";
		}
	}
}