<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('frontchannel-logout.title')}" displayMessage=displayMessage>
            <p>${msg("frontchannel-logout.message")}</p>
            <ul>
                <#list logout.clients as client>
                    <li>
                        ${client.name}
                        <iframe src="${client.frontChannelLogoutUrl}" style="display:none;"></iframe>
                    </li>
                </#list>
            </ul>
            <#if logout.logoutRedirectUri?has_content>
                <script>
                    function readystatechange(event) {
                        if (document.readyState=='complete') {
                            window.location.replace('${logout.logoutRedirectUri}');
                        }
                    }
                    document.addEventListener('readystatechange', readystatechange);
                </script>
                <@components.link href="${logout.logoutRedirectUri}">
                    ${msg("doContinue")}
                </@components.link>
            </#if>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
