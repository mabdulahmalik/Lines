<#macro logo>
    <img src="${url.resourcesPath}/img/logo.png"
         alt="${kcSanitize(msg("loginTitleHtml",(realm.displayNameHtml!'')))?no_esc}"
    >
</#macro>