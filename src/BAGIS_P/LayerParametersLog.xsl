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
      <BODY>
        <H1>
          <xsl:value-of select="HruName"/>
        </H1>
      </BODY>
    </HTML>
  </xsl:template>
</xsl:stylesheet>