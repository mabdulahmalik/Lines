<script setup lang="ts">
import { FwbToggle } from 'flowbite-vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { IconDotsReorder, IconPlusCircle, IconTrash01 } from '@/components/icons/index';
import draggable from 'vuedraggable';
import { FwbInput, FwbSelect, FwbButton } from 'flowbite-vue';
import { ref, computed } from 'vue';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';
import { RequiredPatientData, CreateProcedureCmd, ProcedureFieldPrm, ProcedureType } from '@/api/__generated__/graphql';
import { useForm, useFieldArray } from 'vee-validate';
import * as yup from 'yup';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import ReadOnlyCheckbox from '../partials/ReadonlyCheckbox.vue';
import ReadonlyRadio from '../partials/ReadonlyRadio.vue';

const props = defineProps<{
  name: string;
  type: string;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
}>();

const proceduresStore = useProceduresStore();

interface PF {
  value: {
    name: string;
    required: boolean;
    allowMultiSelect: boolean;
    instruction: string;
    options: string[];
    type: string;
  };
}

// Setting & Required Data
const enablePerformanceReporting = ref(false);
const selectedRequiredData = ref<RequiredPatientData[]>([]);

// Right Panel
const propertyTypesOptions = [
  { value: 'NUMBER', name: 'Number' },
  { value: 'TEXT', name: 'Text' },
  { value: 'LIST', name: 'Options List' },
  { value: 'TOGGLE', name: 'Toggle' },
];

const initialProcedureFields = [
  {
    name: '',
    required: false,
    allowMultiSelect: false,
    instruction: '',
    options: [],
    type: '',
  },
];

//Validations
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const schemaAddProcedure = yup.object({
  procedureFields: yup.array().of(
    yup.object().shape({
      name: yup.string().required('This is required field'),
      required: yup.boolean(),
      allowMultiSelect: yup.boolean(),
      instruction: yup.string(),
      options: yup.array().of(yup.string()),
      type: yup.string().required('This is required field'),
    })
  ),
});
const { handleSubmit: handleSubmitAddProcedure, errors: createProcedureErrors } = useForm({
  initialValues: {
    procedureFields: initialProcedureFields,
  },
  validationSchema: schemaAddProcedure,
  validateOnMount: false, // Disable initial validation
});

const { fields: procedureFields, push: pushToProcedureFields } = useFieldArray('procedureFields');

// Add new property
const addProperty = () => {
  pushToProcedureFields({
    name: '',
    required: false,
    allowMultiSelect: false,
    instruction: '',
    options: [],
    type: '',
  });
};

// Field options
const editingOptionIndex = ref<number | null>(null);
const editingOptionPropertyIndex = ref<number | null>(null);
const toggleEditOption = (propertyIndex: number, optionIndex: number) => {
  editingOptionIndex.value = optionIndex;
  editingOptionPropertyIndex.value = propertyIndex;
};

// Delete an option from the list
const deleteOptionItem = (propertyIndex: number, optionIndex: number) => {
  const options = (procedureFields.value[propertyIndex].value as PF['value']).options;
  if (propertyIndex !== -1 && optionIndex !== -1) {
    options.splice(optionIndex, 1);
  }
};

// Add a new option to the options list
const addOptionToList = (propertyIndex: number) => {
  const newOptions = (procedureFields.value[propertyIndex].value as PF['value']).options;
  newOptions.push(`Option ${newOptions.length + 1}`);
};

const isAddProcedureSubmit = ref(false);

const submittedData = () => {
  isAddProcedureSubmit.value = true;
  handleSubmitAddProcedure(async (values) => {
    const newProcedure: CreateProcedureCmd = {
      name: props.name,
      enablePerformanceReporting: enablePerformanceReporting.value,
      type: props.type as ProcedureType,
      requiredData: [...selectedRequiredData.value],
      fields: values.procedureFields as ProcedureFieldPrm[],
    };
    proceduresStore.createProcedure(newProcedure);
    closeCreateProcedureDrawer();
  })();
};

// drag & drop
const dragOptions = computed(() => {
  return {
    animation: 200,
    group: 'table-items',
    disabled: false,
    ghostClass: 'shadow',
  };
});

const closeCreateProcedureDrawer = () => {
  emit('close');
};
defineExpose({
  submittedData,
});
</script>
<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Left pannel -->
    <div
      class="h-auto lg:h-full w-full lg:w-80 lg:border-r border-slate-300 py-4 overflow-y-auto custom-scroll"
    >
      <div class="px-4">
        <!-- required patient fields  -->
        <AccordionDefault
          id="required_procedure_files"
          active
          title="Required Patient Fields"
          class="pb-2 lg:pb-4 border-slate-300"
        >
          <div class="flex flex-col gap-5 pt-4 px-1">
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.FirstName"
              label="Patient First Name "
              class="font-medium text-sm"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.LastName"
              label="Patient Last Name "
              class="font-medium text-sm"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.DateOfBirth"
              label="Date of Birth"
              class="font-medium text-sm"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.OrderingProvider"
              label="Ordering Provider"
              class="font-medium text-sm"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.Mrn"
              label="MRN"
              class="font-medium text-sm"
            />
          </div>
        </AccordionDefault>

        <!--  Settings  -->
        <AccordionDefault
          id="procedure_settings"
          active
          title="Settings"
          class="pb-4 border-slate-300"
        >
          <div class="pt-4 flex flex-col gap-5">
            <fwb-toggle v-model="enablePerformanceReporting" label="Performance Reporting" />
          </div>
        </AccordionDefault>
      </div>
      <hr class="border-slate-300" />
    </div>

    <!-- Right pannel -->
    <div class="h-full w-full lg:flex-1 py-4 lg:px-10 px-4 overflow-y-auto custom-scroll">
      <div class="text-xl font-semibold">Procedure Fields</div>
      <div>
        <!-- property Form  -->
        <draggable
          v-if="procedureFields"
          v-model="procedureFields"
          item-key="id"
          tag="div"
          v-bind="dragOptions"
          handle=".draggable-handle-icon"
        >
          <template #item="{ element: property, index }">
            <AccordionDefault
              :id="`property-to-add-${index}`"
              custom_header
              :active="true"
              class="pb-2 lg:pb-4 my-8 border-b border-slate-300"
            >
            <template #customHeader="{ open }">
                <div class="w-full flex items-center justify-between bg-slate-50 p-2 rounded-lg">
                  <div class="flex gap-2">
                    <IconDotsReorder class="draggable-handle-icon cursor-grab active:cursor-grabbing" @click.stop />
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="24"
                      height="24"
                      viewBox="0 0 24 24"
                      fill="none"
                    >
                      <path
                        class="transform origin-center transition duration-200 ease-out"
                        :class="{ 'rotate-0': open, 'rotate-180': !open }"
                        d="M18 15L12 9L6 15"
                        stroke="#64748B"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>

                    <div> {{ (property.value as PF['value']).name? (property.value as PF['value']).name: `Property ${index + 1 } Details `}} </div>
                  </div>
                </div>
              </template>
              <form class="flex flex-col gap-4 pt-2">
                <!-- Label -->
                <div>
                  <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                    >Label</label
                  >
                  <fwb-input
                    v-model="(property.value as PF['value']).name"
                    placeholder="Write text here"
                    :class="
                    createProcedureErrors[`procedureFields[${index}].name` as any] && isAddProcedureSubmit
                      ? inputErrorClasses
                      : ''
                  "
                  />
                  <span
                    v-if="
                    createProcedureErrors[`procedureFields[${index}].name` as any] && isAddProcedureSubmit
                  "
                    class="text-sm text-radical-red-600"
                    >{{ createProcedureErrors[`procedureFields[${index}].name` as any] }}</span
                  >
                </div>

                <!-- Instruction -->
                <div>
                  <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                    >Instruction</label
                  >
                  <fwb-input
                    v-model="(property.value as PF['value']).instruction"
                    placeholder="Write text here"
                  />
                </div>

                <!-- Property Type -->
                <div>
                  <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                    >Property Type</label
                  >
                  <fwb-select
                    v-model="(property.value as PF['value']).type"
                    :options="propertyTypesOptions"
                    :class="
                    createProcedureErrors[`procedureFields[${index}].type` as any] && isAddProcedureSubmit
                      ? inputErrorClasses
                      : ''
                  "
                  />
                  <span
                    v-if="
                    createProcedureErrors[`procedureFields[${index}].type` as any] && isAddProcedureSubmit
                  "
                    class="text-sm text-radical-red-600"
                    >{{ createProcedureErrors[`procedureFields[${index}].type` as any] }}</span
                  >
                </div>

                <div class="flex flex-col gap-4">
                  <fwb-toggle v-model="(property.value as PF['value']).required" label="Required" />
                  <fwb-toggle
                    v-if="(property.value as PF['value']).type === 'LIST'"
                    v-model="(property.value as PF['value']).allowMultiSelect"
                    label="Allow Multiple Selections"
                  />
                </div>

                <!-- when property type is Number -->
                <!-- <div v-if="(property.value as PF['value']).type === 'NUMBER'" class="flex gap-6">
                  <div class="flex-1">
                    <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                      >Min. Value</label
                    >
                    <fwb-input type="number" placeholder="numeric values" />
                  </div>
                  <div class="flex-1">
                    <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                      >Max. Value</label
                    >
                    <fwb-input type="number" placeholder="numeric values" />
                  </div>
                </div> -->

                <!-- when property type is OptionList -->
                <div v-if="(property.value as PF['value']).type === 'LIST'">
                  <div class="p-4 gap-1 bg-slate-50 rounded-lg my-6">
                    <draggable
                      v-if="(property.value as PF['value']).options"
                      v-model="(property.value as PF['value']).options"
                      :item-key="Math.random().toFixed(30)"
                      tag="div"
                      v-bind="dragOptions"
                      handle=".draggable-handle-icon"
                    >
                      <template #item="{ index: optionIndex }">
                        <div
                          @click="toggleEditOption(index, optionIndex)"
                          class="flex items-center hover:bg-slate-200 p-2 rounded-lg group cursor-pointer"
                        >
                          <IconDotsReorder
                            class="mr-5 flex-shrink-0 draggable-handle-icon cursor-grab"
                          />
                          <template
                            v-if="
                              editingOptionIndex === optionIndex &&
                              editingOptionPropertyIndex === index
                            "
                          >
                            <fwb-input
                              v-model="(property.value as PF['value']).options[optionIndex]"
                              @click.stop
                              class="w-full"
                              size="sm"
                            />
                            <fwb-button
                              @click.stop="editingOptionIndex = null"
                              class="ms-5 bg-primary-600 hover:bg-brand-600"
                              pill
                              >Save</fwb-button
                            >
                          </template>
                          <template v-else>
                            <ReadOnlyCheckbox
                              v-if="(property.value as PF['value']).allowMultiSelect"
                              :label="(property.value as PF['value']).options[optionIndex]"
                              class="font-medium text-sm my-2"
                            />
                            <ReadonlyRadio
                              v-else
                              :label="(property.value as PF['value']).options[optionIndex]"
                              class="font-medium text-sm my-2"
                            />
                            <IconTrash01
                              @click.stop="deleteOptionItem(index, optionIndex)"
                              class="ms-auto opacity-0 group-hover:opacity-100"
                            />
                          </template>
                        </div>
                      </template>
                    </draggable>

                    <!-- add options  -->
                    <div class="mt-4">
                      <span
                        @click="addOptionToList(index)"
                        class="text-brand-600 text-sm font-semibold cursor-pointer"
                      >
                        + Add option
                      </span>
                    </div>
                  </div>
                </div>
              </form>
            </AccordionDefault>
          </template>
        </draggable>
        <!-- add property  -->
        <div class="mt-4">
          <button @click="addProperty" class="flex items-center text-brand-600 text-sm font-medium">
            <IconPlusCircle class="mr-0.5" /> Add Property
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Select flowbite validition classes */
:deep(.bg-radical-red-50 select) {
  border-color: #e11d47;
  background-color: inherit;
}
</style>
