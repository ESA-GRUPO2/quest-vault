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

WebUI.navigateToUrl('https://questvault.azurewebsites.net/')

WebUI.maximizeWindow(FailureHandling.STOP_ON_FAILURE)

WebUI.click(findTestObject('Object Repository/Page_Home - QuestVault/a_Log in'))

WebUI.setText(findTestObject('Object Repository/Page_- QuestVault/input_Log in_Input.EmailUserName'), 'notAdmin')

WebUI.setEncryptedText(findTestObject('Object Repository/Page_- QuestVault/input_Log in_Input.Password'), 'LUp6avagZuk=')

WebUI.click(findTestObject('Object Repository/Page_- QuestVault/button_Log in'))

WebUI.click(findTestObject('Object Repository/Page_Library - QuestVault/a_MyLibrary'))

WebUI.verifyElementText(findTestObject('Object Repository/Page_Library - QuestVault/h2_No games Added'), 'No games Added!')

WebUI.click(findTestObject('Object Repository/Page_Library - QuestVault/img_MyLibrary_mb-1'))

WebUI.click(findTestObject('Object Repository/Page_Library - QuestVault/a_Account'))

WebUI.click(findTestObject('Object Repository/Page_Account - QuestVault/button_Connect to Steam'))

WebUI.waitForPageLoad(2)

WebUI.setText(findTestObject('Object Repository/Page_Account - QuestVault/input_Insert Steam Id_steamId'), '76561198140392808')

WebUI.click(findTestObject('Object Repository/Page_Account - QuestVault/button_Import Library'))

WebUI.verifyElementNotPresent(findTestObject('Page_Library - QuestVault/h2_No games Added'), 0)

WebUI.closeBrowser()

