<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=false; section>
    <#if section = "form">
        <@components.cardMain header="" hasBackToLoginLink=true displayMessage=displayMessage>
            <div class="my-5">
                ${msg("magicLinkFailLogin")}
            </div>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>