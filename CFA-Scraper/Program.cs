using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using CFA_Scraper.Model;
using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

var writer = new StreamWriter("cfaMembers.csv");
var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
csvWriter.WriteField("First Name");
csvWriter.WriteField("Last Name");
csvWriter.WriteField("Position");
csvWriter.WriteField("Employment");
csvWriter.WriteField("Location");
csvWriter.NextRecord();

var locationName = "New York";
var pages = 834;
int page = 1;
var cfaMember = new CFAMember();
var cfaMembers = new List<CFAMember>();

var url = $"https://directory.cfainstitute.org/search?location={locationName}";

WebDriver driver = new ChromeDriver();
driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
driver.Navigate().GoToUrl(url);
var agreeCheckbox = driver.FindElement(By.XPath("/html/body/div/main/div[2]/section/form/div[2]/div[3]/div/div/label"));
agreeCheckbox.Click();
var searchButton = driver.FindElement(By.XPath("/html/body/div/main/div[2]/section/form/div[2]/div[4]/button"));
searchButton.Click();

while (page < pages)
{

    var members = driver.FindElement(By.XPath("/html/body/div/main/div[2]/section[2]/div/ul"));

    var listOfMembers = members.FindElements(By.TagName("li"));

    var index = 1;

    foreach (var ele in listOfMembers)
    {
        var memberData = ele.FindElements(By.TagName("p")).Count;
        var memberFirstName = string.Empty;
        var memberLastName = string.Empty;
        var memberCredential = string.Empty;
        var memberEmployment = string.Empty;
        var memberPosition = string.Empty;
        var memberLocation = string.Empty;
        

        if (memberData == 3)
        {
            memberFirstName = ele.FindElement(By.XPath($"/html/body/div/main/div[2]/section[2]/div/ul/li[{index}]/div/div/p[1]/span[1]")).Text;
            memberLastName = ele.FindElement(By.XPath($"/html/body/div/main/div[2]/section[2]/div/ul/li[{index}]/div/div/p[1]/span[2]")).Text;
            memberLocation = ele.FindElement(By.ClassName("person-address")).Text;



            cfaMember.MemberFirstName = memberFirstName;
            cfaMember.MemberLastName = memberLastName;
            cfaMember.MemberLocation = memberLocation;

        }
        else
        {
            memberFirstName = ele.FindElement(By.XPath($"/html/body/div/main/div[2]/section[2]/div/ul/li[{index}]/div/div/p[1]/span[1]")).Text;
            memberLastName = ele.FindElement(By.XPath($"/html/body/div/main/div[2]/section[2]/div/ul/li[{index}]/div/div/p[1]/span[2]")).Text;
            memberLocation = ele.FindElement(By.ClassName("person-address")).Text;
            memberEmployment = ele.FindElement(By.XPath($"/html/body/div/main/div[2]/section[2]/div/ul/li[{index}]/div/div/p[3]")).Text;
            memberPosition = ele.FindElement(By.XPath($"/html/body/div/main/div[2]/section[2]/div/ul/li[{index}]/div/div/p[2]")).Text;

            cfaMember.MemberFirstName = memberFirstName;
            cfaMember.MemberLastName = memberLastName;
            cfaMember.MemberLocation = memberLocation;
            cfaMember.MemberEmployment = memberEmployment;
            cfaMember.MemberPosition = memberPosition;


        }

        csvWriter.WriteField(cfaMember.MemberFirstName);
        csvWriter.WriteField(cfaMember.MemberLastName);
        csvWriter.WriteField(cfaMember.MemberPosition);
        csvWriter.WriteField(cfaMember.MemberEmployment);
        csvWriter.WriteField(cfaMember.MemberLocation);
        csvWriter.NextRecord();
        

        index++;
    }
    
    page++;

    writer.Flush();


    driver.Navigate().GoToUrl($"https://directory.cfainstitute.org/search?location={locationName}&page={page}");
}




