<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/LayerParametersLog">
    <HTML>
      <head>
        <title>
          Layer Parameters Log for <xsl:value-of select="HruName"/>
        </title>
        <style type="text/css">
          .style1
          {
          text-align: center;
          font-family: Arial, Helvetica, sans-serif;
          font-weight: bold;
          padding: 1px 4px;
          }
          .style2
          {
          width: 600px;
          border-collapse: collapse;
          }
          .style3
          {
          font-family: Arial, Helvetica, sans-serif;
          }


          .style12
          {
          font-family: Arial, Helvetica, sans-serif;
          width: 137px;
          font-weight: bold;
          }

          .style14
          {
          font-family: Arial, Helvetica, sans-serif;
          width: 250px;
          border: 1px solid black;
          padding: 1px 4px;
          }

          .style15
          {
          font-family: Arial, Helvetica, sans-serif;
          width: 250px;
          height: 20px;
          }

        </style>
      </head>
      <body>
        <p class="style1">
          Layer Parameters Log for <xsl:value-of select="HruName"/>
        </p>
        <table class="style2">
          <tr>
            <td class="style12" colspan="2"> </td>
          </tr>
          <tr>
            <td class="style12">
              AOI name:
            </td>
            <td class="style3"><xsl:value-of select="AoiName"/></td>
          </tr>
          <tr>
            <td class="style12">AOI folder path:</td>
            <td class="style3"><xsl:value-of select="AoiPath"/></td>
          </tr>
          <tr>
            <td class="style12">
              HRU name:
            </td>
            <td class="style3">
              <xsl:value-of select="HruName"/>
            </td>
          </tr>
          <tr>
            <td colspan="2"></td>
          </tr>
        </table>
        <p></p>
        <xsl:for-each select="LayerParameters/LayerParameter">
        <table class="style2">
          <tr>
            <td class="style14" colspan ="2">
              <b>Output parameter name:</b> <xsl:value-of select="Name"/></td>
          </tr>
            <tr>
              <td class="style14" colspan ="2">
                <b>Date calculated:</b> <xsl:value-of select="DateCalculatedText"/>
              </td>
            </tr>
            <tr>
            <td class="style14" colspan = "2">
              <b>Input layer path:</b> <xsl:value-of select="LayerPath"/></td>
          </tr>
          <tr>
            <td class="style14" colspan = "2">
              <b>Input layer type:</b> <xsl:value-of select="LayerType"/>
          </td>
          </tr>
          <tr>
            <td class="style14">
              <b>Layer value</b>
          </td>
            <td class="style14">
              <b>Parameter value</b>
          </td>
          </tr>
            <xsl:for-each select="LayerValuesList/string">
              <xsl:variable name="vPos" select="position()" />
          <tr>
            <td class="style14">
              <xsl:value-of select="text()"/>
            </td>
            <td class="style14"><xsl:value-of select="../../ParameterValuesList/*[$vPos]" />
            </td>
          </tr>
            </xsl:for-each>
        </table>
          <p></p>
        </xsl:for-each>
      </body>
    </HTML>
  </xsl:template>
</xsl:stylesheet>