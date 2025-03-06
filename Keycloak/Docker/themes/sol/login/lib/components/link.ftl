<#macro link href props...>
    <a
        href=${href}

        <#list props as attrName, attrVal>
            ${attrName}="${attrVal}"
        </#list>
    >
        <#nested>
    </a>
</#macro>