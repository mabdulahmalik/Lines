<#import "../components.ftl" as components>

<#macro userProfileFormFields>
    <#assign currentGroup="">

    <#list profile.attributes as attribute>
        <#if currentGroup != attribute.group!"">
            <#assign currentGroup=attribute.group!"">

            <#if currentGroup != "" >
                <div
<#--					<#list group.html5DataAnnotations as key, value>-->
<#--						data-${key}="${value}"-->
<#--					</#list>-->
                >
                    <h4>
                        ${advancedMsg(currentGroup.displayHeader!"")}
                    </h4>

                    <#if currentGroup.displayDescription?has_content>
                        <p>
							${advancedMsg(currentGroup.displayDescription!"")}
                        </p>
                    </#if>
                </div>
            </#if>
        </#if>

        <#nested "beforeField" attribute>
        <@inputFieldByType attribute=attribute/>
        <#nested "afterField" attribute>
    </#list>

	<#list profile.html5DataAnnotations?keys as key>
		<script type="module" src="${url.resourcesPath}/js/${key}.js"></script>
	</#list>
</#macro>

<#macro inputFieldByType attribute>
	<#switch attribute.annotations.inputType!''>
		<#case 'textarea'>
			<@inputTag attribute=attribute/>
			<#break>
		<#case 'select'>
		<#case 'multiselect'>
<#--			un-supported as of now -->
<#--			<@selectTag attribute=attribute/> -->
			<#break>
		<#case 'select-radiobuttons'>
		<#case 'multiselect-checkboxes'>
<#--		un-supported as of now -->
<#--		<@inputTagSelects attribute=attribute/>-->
			<#break>
		<#default>
			<@inputTag attribute=attribute/>
	</#switch>
</#macro>

<#macro inputTag attribute>
    <@components.formField
		type="${attribute.annotations.inputType???then(attribute.annotations.inputType?starts_with('html5-')?then(attribute.annotations.inputType[6..], attribute.annotations.inputType), 'text')}"
		id="${attribute.name}"
		name="${attribute.name}"
		value="${(attribute.value!'')}"
		autocomplete="${attribute.autocomplete!''}"
		booleanAttributes=["${attribute.required?then('required', '')}", "${attribute.readOnly?then('disabled', '')}"]

		labelText="${attribute.displayName???then(advancedMsg(attribute.displayName), '')}"
		hasError=messagesPerField.existsError(attribute.name)
		errorMessage="${kcSanitize(messagesPerField.get(attribute.name))?no_esc}"

		helperTextBefore="${attribute.annotations.inputHelperTextBefore???then(kcSanitize(advancedMsg(attribute.annotations.inputHelperTextBefore))?no_esc, '')}"
		helperTextAfter="${attribute.annotations.inputHelperTextAfter???then(kcSanitize(advancedMsg(attribute.annotations.inputHelperTextAfter))?no_esc, '')}"

		placeholder="${attribute.annotations.inputTypePlaceholder!''}"
		pattern="${attribute.annotations.inputTypePattern!''}"
		size="${attribute.annotations.inputTypeSize!''}"
		maxlength="${attribute.annotations.inputTypeMaxlength!''}"
		minlength="${attribute.annotations.inputTypeMinlength!''}"
		max="${attribute.annotations.inputTypeMax!''}"
		min="${attribute.annotations.inputTypeMin!''}"
		step="${attribute.annotations.inputTypeStep!''}"

		cols="${attribute.annotations.inputTypeCols!''}"
		rows="${attribute.annotations.inputTypeRows!''}"
	/>
</#macro>
