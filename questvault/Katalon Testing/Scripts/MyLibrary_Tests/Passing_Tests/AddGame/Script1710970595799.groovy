import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testng.keyword.TestNGBuiltinKeywords as TestNGKW
import com.kms.katalon.core.testobject.TestObject as TestObject
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

WebUI.openBrowser('')

WebUI.navigateToUrl('https://localhost:7074/')

WebUI.maximizeWindow(FailureHandling.STOP_ON_FAILURE)

WebUI.click(findTestObject('Object Repository/Page_Home - QuestVault/a_Log in'))

WebUI.setText(findTestObject('Object Repository/Page_- QuestVault/input_Log in_Input.EmailUserName'), 'tiagojoaobatista4@gmail.com')

WebUI.setEncryptedText(findTestObject('Object Repository/Page_- QuestVault/input_Log in_Input.Password'), 'LUp6avagZuk=')

WebUI.click(findTestObject('Object Repository/Page_- QuestVault/button_Log in'))

WebUI.click(findTestObject('Object Repository/Page_Home - QuestVault/a_Games'))

WebUI.click(findTestObject('Object Repository/Page_Results - QuestVault/img'))

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/button_Add Game'))

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/div_Game Status'))

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/span_CompleteBeat your main objective'))

WebUI.selectOptionByValue(findTestObject('Object Repository/Page_Game - QuestVault/select_Playing                            C_51d264'), 
    'Complete', true)

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/button_Save'))

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/a_MyLibrary'))

WebUI.verifyElementPresent(findTestObject('Object Repository/Page_Library - QuestVault/img'), 0)

WebUI.closeBrowser()

