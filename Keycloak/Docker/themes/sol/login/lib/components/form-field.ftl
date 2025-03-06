<#macro formField
<#--Formfied required attributes-->
type
id
name
<#--Formfield boolean attributes-->
booleanAttributes=[]

labelText=""
hasError=false
errorMessage=""

helperTextBefore=""
helperTextAfter=""

<#--{'': 'Select an option', 'value-1': 'Label 1', 'value-2': 'Label 2'}-->
selectOptions={}
values=[]

<#--Formfield other key="value" attributes-->
otherAttributes...
>
    <#switch type>
        <#case "select">
            <div class="my-4">
                <@label for=id labelText=labelText forRequiredField=booleanAttributes?seq_contains("required") />
                <@helperText text=helperTextBefore />
                <select
                        id="${id}"
                        name="${name}"
                        <@attributes attributes=otherAttributes booleanAttributes=booleanAttributes />
                        class="form-select ${hasError?then('is-invalid', '')}"
                >
                    <#if selectOptions??>
                        <#list selectOptions as value, label>
                            <option value="${value}" ${values?seq_contains(value)?then('selected', '')}>${label}</option>
                        </#list>
                    </#if>
                </select>
                <@helperText text=helperTextAfter />
                <@error for=id hasError=hasError errorMessage=errorMessage />
            </div>
            <#break>
        <#case "textarea">
            <div class="my-4">
                <@label for=id labelText=labelText forRequiredField=booleanAttributes?seq_contains("required") />
                <@helperText text=helperTextBefore />
                <textarea
                    id="${id}"
                    name="${name}"
                    <@attributes attributes=otherAttributes booleanAttributes=booleanAttributes />
                    class="form-control ${hasError?then('is-invalid', '')}"
                >${otherAttributes.value!''}</textarea>
                <@helperText text=helperTextAfter />
                <@error for=id hasError=hasError errorMessage=errorMessage />
            </div>
            <#break>
        <#case "checkbox">
        <#case "radio">
            <div class="mb-4">
                <input type="${type}"
                       id="${id}"
                       name="${name}"
                        <@attributes attributes=otherAttributes booleanAttributes=booleanAttributes />
                       class=""
                />
                <@label for=id labelText=labelText forRequiredField=booleanAttributes?seq_contains("required") />
            </div>
            <#break>
        <#case "password">
            <div class="my-4">
                <@label for=id labelText=labelText forRequiredField=booleanAttributes?seq_contains("required") />
                <@helperText text=helperTextBefore />
                <div class="" style="position: relative; padding: 0; display: flex; align-items: center">
                    <input type="${type}"
                           id="${id}"
                           name="${name}"
                            <@attributes attributes=otherAttributes booleanAttributes=booleanAttributes />
                           class="${hasError?then('is-invalid', '')}"
                           style="flex-grow:2;"
                    />
                    <span style="position: absolute; right: 1rem;" onclick="toggle(this, '${id}')">
                        <svg class="" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 576 512">
                            <path fill="black"
                                  d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"/>
                        </svg>
                        <svg class="hidden" xmlns="http://www.w3.org/2000/svg" width="20" height="20"
                             viewBox="0 0 640 512">
                            <path fill="black"
                                  d="M38.8 5.1C28.4-3.1 13.3-1.2 5.1 9.2S-1.2 34.7 9.2 42.9l592 464c10.4 8.2 25.5 6.3 33.7-4.1s6.3-25.5-4.1-33.7L525.6 386.7c39.6-40.6 66.4-86.1 79.9-118.4c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C465.5 68.8 400.8 32 320 32c-68.2 0-125 26.3-169.3 60.8L38.8 5.1zm151 118.3C226 97.7 269.5 80 320 80c65.2 0 118.8 29.6 159.9 67.7C518.4 183.5 545 226 558.6 256c-12.6 28-36.6 66.8-70.9 100.9l-53.8-42.2c9.1-17.6 14.2-37.5 14.2-58.7c0-70.7-57.3-128-128-128c-32.2 0-61.7 11.9-84.2 31.5l-46.1-36.1zM394.9 284.2l-81.5-63.9c4.2-8.5 6.6-18.2 6.6-28.3c0-5.5-.7-10.9-2-16c.7 0 1.3 0 2 0c44.2 0 80 35.8 80 80c0 9.9-1.8 19.4-5.1 28.2zm51.3 163.3l-41.9-33C378.8 425.4 350.7 432 320 432c-65.2 0-118.8-29.6-159.9-67.7C121.6 328.5 95 286 81.4 256c8.3-18.4 21.5-41.5 39.4-64.8L83.1 161.5C60.3 191.2 44 220.8 34.5 243.7c-3.3 7.9-3.3 16.7 0 24.6c14.9 35.7 46.2 87.7 93 131.1C174.5 443.2 239.2 480 320 480c47.8 0 89.9-12.9 126.2-32.5zm-88-69.3L302 334c-23.5-5.4-43.1-21.2-53.7-42.3l-56.1-44.2c-.2 2.8-.3 5.6-.3 8.5c0 70.7 57.3 128 128 128c13.3 0 26.1-2 38.2-5.8z"/>
                        </svg>
                    </span>
                </div>
                <@helperText text=helperTextAfter />
                <@error for=id hasError=hasError errorMessage=errorMessage />
            </div>
            <#break>
        <#default>
            <div class="my-4 flex flex-col space-y-1">
                <@label for=id labelText=labelText forRequiredField=booleanAttributes?seq_contains("required") />
                <@helperText text=helperTextBefore />
                <input type="${type}"
                       id="${id}"
                       name="${name}"
                        <@attributes attributes=otherAttributes booleanAttributes=booleanAttributes />
                       class="${hasError?then('is-invalid', '')}"
                />
                <@helperText text=helperTextAfter />
                <@error for=id hasError=hasError errorMessage=errorMessage />
            </div>
    </#switch>
</#macro>

<#macro label for labelText="" forRequiredField=false>
    <#if labelText?has_content>
        <label for="${for}" class="">
            ${labelText?cap_first}
            <#if forRequiredField>
                <span class="text-invalid">*</span>
            </#if>
        </label>
    </#if>
</#macro>

<#macro attributes attributes booleanAttributes=[]>
    <#if (attributes?size > 0)>
        <#list attributes as name, value>
            <#if value?has_content>
                ${name}="${value}"
            </#if>
        </#list>
    </#if>
    <#if (booleanAttributes?size > 0)>
        <#list booleanAttributes as attribute>
            <#if attribute?has_content>
                ${attribute}
            </#if>
        </#list>
    </#if>
</#macro>

<#macro error for hasError errorMessage="">
    <#if hasError>
        <label for="${for}" class="text-invalid text-sm">
            ${errorMessage!"field error"}
        </label>
    </#if>
</#macro>

<#macro helperText text="">
    <#if text?has_content>
        <div class="text-sm">
            ${text}
        </div>
    </#if>
</#macro>