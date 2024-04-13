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

WebUI.maximizeWindow()

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/a_Log in'))

WebUI.setText(findTestObject('Object Repository/Page_Game - QuestVault/input_Log in_Input.EmailUserName'), 'tBatista')

WebUI.setEncryptedText(findTestObject('Object Repository/Page_Game - QuestVault/input_Log in_Input.Password'), 'LUp6avagZuk=')

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/button_Log in'))

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/img'))

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/a_The Witcher 3 Wild Hunt - Game of the Yea_879e5d'))

WebUI.switchToWindowTitle('The Witcher 3: Wild Hunt - Game of the Year Edition - Launch Trailer (Official) - YouTube')

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/div_Aceitar tudo_yt-spec-touch-feedback-sha_631d3e'))

WebUI.delay(3)

WebUI.verifyElementText(findTestObject('Object Repository/Page_Game - QuestVault/yt-formatted-string_The Witcher 3 Wild Hunt_9cf09b'), 
    'The Witcher 3: Wild Hunt - Game of the Year Edition - Launch Trailer (Official)')

WebUI.click(findTestObject('Object Repository/Page_Game - QuestVault/video_PT_video-stream html5-main-video'))

WebUI.closeBrowser()

