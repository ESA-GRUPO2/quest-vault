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

WebUI.click(findTestObject('Object Repository/Page_- QuestVault/main_Log in                                _4f0d7d'))

WebUI.setText(findTestObject('Object Repository/Page_- QuestVault/input_Log in_Input.EmailUserName'), 'tBatista')

WebUI.click(findTestObject('Object Repository/Page_- QuestVault/div_Log in_row-login'))

WebUI.setText(findTestObject('Object Repository/Page_- QuestVault/input_Log in_Input.EmailUserName'), 'notAdmin')

WebUI.setEncryptedText(findTestObject('Object Repository/Page_- QuestVault/input_Log in_Input.Password'), 'LUp6avagZuk=')

WebUI.click(findTestObject('Object Repository/Page_- QuestVault/button_Log in'))

WebUI.click(findTestObject('Object Repository/Page_Library - QuestVault/img'))

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/img_Shooter_img-size-sm'))

WebUI.switchToWindowTitle('Team Fortress 2 no Steam')

WebUI.closeBrowser()

