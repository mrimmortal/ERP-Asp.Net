using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;


public class clsErrorLogs
{
    public static string strDirctoryPath = ConfigurationManager.AppSettings["ErrorLogDirectory"].ToString();
    string strFileName = string.Empty;
    string strFilePath = string.Empty;
    string strErrorContent = string.Empty;
    string strHeaderName = string.Empty;

    public void WriteErrorLogs(string strLoginUser, string strClassName, string strMethodName, string strErrorMessage, string strIPAddress)
    {
        try
        {
            string strTime = DateTime.Now.ToString("HH:mm");
            strFileName = "ErrorLog_" + DateTime.Now.ToString("yyyy-MMM-dd") + ".csv";
            strFilePath = strDirctoryPath + "\\" + strFileName;

            strClassName = strClassName.Replace(",", " ");
            strMethodName = strMethodName.Replace(",", " ");
            strErrorMessage = strErrorMessage.Replace(",", " ");
            strLoginUser = strLoginUser.Replace(",", " ");

            strErrorContent = strTime + "," + strLoginUser + "," + strClassName + "," + strMethodName + "," + strErrorMessage + "," + strIPAddress + "\r\n";

            if (!Directory.Exists(strDirctoryPath))
                Directory.CreateDirectory(strDirctoryPath);

            if (!File.Exists(strFilePath))
            {
                strHeaderName = "Time,LoginUser,ClassName,MethodName,ErrorMessage,IP Address \r\n";
                File.WriteAllText(strFilePath, strHeaderName);
            }
            if (!string.IsNullOrEmpty(strFilePath))
                File.AppendAllText(strFilePath, strErrorContent);
        }
        catch (Exception ex)
        {

        }
    }
}

