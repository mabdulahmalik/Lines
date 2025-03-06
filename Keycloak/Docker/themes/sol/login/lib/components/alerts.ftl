<#macro alerts displayMessage>
    <#-- App-initiated actions should not see warning messages about the need to complete the action -->
    <#-- during login.                                                                               -->
    <#if displayMessage && message?has_content && (message.type != 'warning' || !isAppInitiatedAction??)>
        <div>
            <@alertIcons/>
            <#switch message.type>
                <#case 'success'>
                    <@alertSuccess>
                        ${kcSanitize(message.summary)?no_esc}
                    </@alertSuccess>
                    <#break>
                <#case 'warning'>
                    <@alertWarning>
                        ${kcSanitize(message.summary)?no_esc}
                    </@alertWarning>
                    <#break>
                <#case 'error'>
                    <@alertDanger>
                        ${kcSanitize(message.summary)?no_esc}
                    </@alertDanger>
                    <#break>
                <#case 'info'>
                    <@alertInfo>
                        ${kcSanitize(message.summary)?no_esc}
                    </@alertInfo>
                    <#break>
                <#default>
                    <@alertPrimary>
                        ${kcSanitize(message.summary)?no_esc}
                    </@alertPrimary>
            </#switch>
        </div>
    </#if>
</#macro>

<#macro alertIcons>
    <svg xmlns="http://www.w3.org/2000/svg" class="hidden">
        <symbol id="check-circle-fill" viewBox="0 0 16 16">
            <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
        </symbol>
        <symbol id="info-fill" viewBox="0 0 16 16">
            <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/>
        </symbol>
        <symbol id="exclamation-triangle-fill" viewBox="0 0 16 16">
            <path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
        </symbol>
    </svg>
</#macro>



<#macro alertPrimary>
    <div class="px-2 py-2 rounded-md text-sm font-bold text-alert-primary border border-alert-primary flex justify-start items-center space-x-2 rounded-0" role="alert">
        <svg width="1rem" height="1rem" class="fill-alert-primary" role="img" aria-label="Info:"><use xlink:href="#info-fill"/></svg>
        <div>
            <#nested>
        </div>
    </div>
</#macro>
<#macro alertSuccess>
    <div class="px-2 py-2 rounded-md text-sm font-bold text-alert-success border border-alert-success flex justify-start items-center space-x-2 rounded-0" role="alert">
        <svg width="1rem" height="1rem" class="fill-alert-success" role="img" aria-label="Success:"><use xlink:href="#check-circle-fill"/></svg>
        <div>
            <#nested>
        </div>
    </div>
</#macro>
<#macro alertWarning>
    <div class="px-2 py-2 rounded-md text-sm font-bold text-alert-warning border border-alert-warning flex justify-start items-center space-x-2 rounded-0" role="alert">
        <svg width="1rem" height="1rem" class="fill-alert-warning" role="img" aria-label="Warning:"><use xlink:href="#exclamation-triangle-fill"/></svg>
        <div>
            <#nested>
        </div>
    </div>
</#macro>
<#macro alertDanger>
    <div class="px-2 py-2 rounded-md text-sm font-bold text-alert-danger border border-alert-danger flex justify-start items-center space-x-2 rounded-0" role="alert">
        <svg width="1rem" height="1rem" class="fill-alert-danger" role="img" aria-label="Danger:"><use xlink:href="#exclamation-triangle-fill"/></svg>
        <div>
            <#nested>
        </div>
    </div>
</#macro>
<#macro alertInfo>
    <div class="px-2 py-2 rounded-md text-sm font-bold text-alert-info border border-alert-info flex justify-start items-center space-x-2 rounded-0" role="alert">
        <svg width="1rem" height="1rem" class="fill-alert-info" role="img" aria-label="Info:"><use xlink:href="#info-fill"/></svg>
        <div>
            <#nested>
        </div>
    </div>
</#macro>