<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<?mso-application progid="Excel.Sheet"?>
<xsl:stylesheet version="1.0"
    xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
 xmlns:user="urn:my-scripts"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:x="urn:schemas-microsoft-com:office:excel"
 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" >

  <xsl:template match="/">
    <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
      xmlns:o="urn:schemas-microsoft-com:office:office"
      xmlns:x="urn:schemas-microsoft-com:office:excel"
      xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
      xmlns:html="http://www.w3.org/TR/REC-html40">
      <DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
        <Author>Joe Cheng (Multek)</Author>
        <LastAuthor>Joe Cheng (Multek)</LastAuthor>
        <Created>2014-02-17T04:43:22Z</Created>
        <LastSaved>2014-02-19T09:41:41Z</LastSaved>
        <Version>12.00</Version>
      </DocumentProperties>
      <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
        <WindowHeight>10005</WindowHeight>
        <WindowWidth>10005</WindowWidth>
        <WindowTopX>120</WindowTopX>
        <WindowTopY>135</WindowTopY>
        <ActiveSheet>1</ActiveSheet>
        <ProtectStructure>False</ProtectStructure>
        <ProtectWindows>False</ProtectWindows>
      </ExcelWorkbook>
      <Styles>
        <Style ss:ID="Default" ss:Name="Normal">
          <Alignment ss:Vertical="Bottom"/>
          <Borders/>
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="8" ss:Color="#000000"/>
        </Style>
        <Style ss:ID="s62">
          <Font ss:FontName="Arial" x:Family="Swiss" ss:Size="8" ss:Color="#FF0000"/>
          <Interior ss:Color="#FFFF00" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s63">
          <Interior ss:Color="#FFFF00" ss:Pattern="Solid"/>
        </Style>
      </Styles>      
      <xsl:apply-templates/>
    </Workbook>
  </xsl:template>


  <xsl:template match="/*">
    <Worksheet>
      <xsl:attribute name="ss:Name">
        <xsl:value-of select="local-name(/*/*)"/>
      </xsl:attribute>
      <Table x:FullColumns="1" x:FullRows="1">
        <Row>
          <xsl:for-each select="*[position() = 1]/*">
            <Cell>
              <Data ss:Type="String">
                <xsl:value-of select="local-name()"/>
              </Data>
            </Cell>
          </xsl:for-each>
        </Row>
        <xsl:apply-templates/>
      </Table>
    </Worksheet>
  </xsl:template>


  <xsl:template match="/*/*">
    <Row>
      <xsl:apply-templates/>
    </Row>
  </xsl:template>


  <xsl:template match="/*/*/*">
    <Cell>
      <Data ss:Type="String">
        <xsl:value-of select="."/>
      </Data>
    </Cell>
  </xsl:template>


</xsl:stylesheet>