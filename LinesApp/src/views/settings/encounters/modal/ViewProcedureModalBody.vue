<script setup lang="ts">
import { FwbToggle } from 'flowbite-vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { IconDotsReorder, IconPlusCircle, IconTrash01, IconEdit03 } from '@/components/icons/index';
import draggable from 'vuedraggable';
import { FwbInput, FwbSelect, FwbButton, FwbTooltip, FwbBadge } from 'flowbite-vue';
import { ref, computed, onMounted, nextTick } from 'vue';
import CustomCheckbox from '@/components/form/CustomCheckbox.vue';
import {
  RequiredPatientData,
  Procedure,
  ModifyProcedureCmd,
  ModifyProcedureFieldPrm,
  ModifyProcedureFieldOptionPrm,
  ProcedureField,
  ProcedureFieldSetting,
  ProcedureFieldOption,
  ProcedureSetting,
  ProcedureType,
} from '@/api/__generated__/graphql';
import { useForm, useFieldArray } from 'vee-validate';
import * as yup from 'yup';
import ReadonlyCheckbox from '../partials/ReadonlyCheckbox.vue';
import ReadonlyRadio from '../partials/ReadonlyRadio.vue';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import DeleteProcedureFieldModal from './DeleteProcedureFieldModal.vue';
import ArchiveProcedureFieldModal from './ArchiveProcedureFieldModal.vue';
import RestoreProcedureFieldModal from './RestoreProcedureFieldModal.vue';
import DeleteProcedureFieldOptionModal from './DeleteProcedureFieldOptionModal.vue';
import ArchiveProcedureFieldOptionModal from './ArchiveProcedureFieldOptionModal.vue';
import RestoreProcedureFieldOptionModal from './RestoreProcedureFieldOptionModal.vue';
import { IconToggle, IconNumber, IconList, IconText } from '@/components/icons/index';
import DateTimeFormatter from '@/utils/dateTimeFormatter';

const props = defineProps<{
  name: string;
  procedure: Procedure;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
}>();

interface PF {
  value: {
    id: string | null;
    archived: boolean;
    name: string;
    required: boolean;
    allowMultiSelect: boolean;
    instruction: string;
    options: ModifyProcedureFieldOptionPrm[];
    type: string;
  };
}

const proceduresStore = useProceduresStore();

const isProcedureArchived = computed(() => {
  if (props.procedure.archived) return true;
  return false;
});

// Setting & Required Data
const enablePerformanceReporting = ref(false);
const isInsertion = ref(false);
const isRemoval = ref(false);
const selectedRequiredData = ref<RequiredPatientData[]>([]);
onMounted(() => {
  enablePerformanceReporting.value =
    props.procedure.settings?.some(
      (setting: string | null) => setting === ProcedureSetting.PerformanceReporting
    ) || false;
  isInsertion.value = props.procedure.type === ProcedureType.Insertion;
  isRemoval.value = props.procedure.type === ProcedureType.Removal;
  if (props.procedure.requiredData) {
    selectedRequiredData.value = props.procedure.requiredData.filter(
      (item): item is RequiredPatientData => item !== null
    );
  }
});

// Right Panel
const propertyTypesOptions = [
  { value: 'NUMBER', name: 'Number' },
  { value: 'TEXT', name: 'Text' },
  { value: 'LIST', name: 'Options List' },
  { value: 'TOGGLE', name: 'Toggle' },
];
// const properties = ref(props.procedure.fields);
const properties = ref(JSON.parse(JSON.stringify(props.procedure.fields)));

const initialProcedureFields = properties.value.map((field: ProcedureField) => ({
  archived: field.archived,
  id: field.id,
  name: field.name,
  required: field.settings?.includes(ProcedureFieldSetting.Required),
  allowMultiSelect: field.settings?.includes(ProcedureFieldSetting.MultiSelect),
  instruction: field.instruction,
  options: field.options?.map(({ __typename, references, ...rest }: any) => rest),
  type: field.type,
}));

//Validations
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const schemaUpdateProcedure = yup.object({
  procedureFields: yup.array().of(
    yup.object().shape({
      id: yup.string().nullable(),
      name: yup.string().required('This is required field'),
      required: yup.boolean(),
      allowMultiSelect: yup.boolean(),
      archived: yup.boolean(),
      instruction: yup.string(),
      options: yup
        .array()
        .of(
          yup.object({ id: yup.string().nullable(), archived: yup.boolean(), text: yup.string() })
        ),
      type: yup.string().required('This is required field'),
    })
  ),
});
const { handleSubmit: handleSubmitUpdateProcedure, errors: updateProcedureErrors } = useForm({
  initialValues: {
    procedureFields: initialProcedureFields,
  },
  validationSchema: schemaUpdateProcedure,
  validateOnMount: false, // Disable initial validation
});

const {
  fields: procedureFields,
  push: pushToProcedureFields,
  remove: removeProcedureField,
} = useFieldArray('procedureFields');

// Add new property
const addProperty = () => {
  editingPropertyIndexes.value.push(procedureFields.value.length);
  pushToProcedureFields({
    archived: false,
    id: null,
    name: '',
    required: false,
    allowMultiSelect: false,
    instruction: '',
    options: [],
    type: '',
  });
};

const editingPropertyIndexes = ref<number[]>([]);
// Edit the property
const handleClickEditProperty = (propertyIdx: number) => {
  editingPropertyIndexes.value.push(propertyIdx);
};

// Field options
const editingOptionIndex = ref<number | null>(null);
const editingOptionPropertyIndex = ref<number | null>(null);

const toggleEditOption = (propertyIndex: number, optionIndex: number) => {
  editingOptionIndex.value = optionIndex;
  editingOptionPropertyIndex.value = propertyIndex;
};

// Add a new option to the options list
const addOptionToList = (propertyIndex: number) => {
  const newOptions = (procedureFields.value[propertyIndex].value as PF['value']).options;
  newOptions.push({ id: null, archived: false, text: `Option ${newOptions.length + 1}` });
};

const isUpdateProcedureSubmit = ref(false);

const submittedData = () => {
  isUpdateProcedureSubmit.value = true;
  handleSubmitUpdateProcedure(async (values) => {
    const modifiedProcedure: ModifyProcedureCmd = {
      id: props.procedure.id,
      name: props.name,
      enablePerformanceReporting: enablePerformanceReporting.value,
      requiredData: [...selectedRequiredData.value],
      fields: values.procedureFields as ModifyProcedureFieldPrm[],
    };
    proceduresStore.modifyProcedure(modifiedProcedure);
    proceduresStore.clearSelectedProcedure();
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

// Delete, Archive & Restore Procedure Field
const actionableProcedureField = ref<ProcedureField>();

const deleteProcedureFieldModal = ref<InstanceType<typeof DeleteProcedureFieldModal>>();
const archiveProcedureFieldModal = ref<InstanceType<typeof ArchiveProcedureFieldModal>>();
const restoreProcedureFieldModal = ref<InstanceType<typeof RestoreProcedureFieldModal>>();

const handleDeleteProperty = (procedureField: ProcedureField, index: number) => {
  const storeProcedureField = props.procedure.fields?.find(
    (field) => field?.id === procedureField.id
  );
  if (!storeProcedureField) {
    removeProcedureField(index);
    return;
  }
  actionableProcedureField.value = storeProcedureField;
  if (storeProcedureField.references > 0) {
    nextTick(() => {
      archiveProcedureFieldModal.value?.setModalOpen(true);
    });
  } else {
    nextTick(() => {
      deleteProcedureFieldModal.value?.setModalOpen(true);
    });
  }
};
function deleteProcedureField(fieldToDelete: ProcedureField) {
  const index = procedureFields.value.findIndex(
    (field) => (field.value as PF['value']).id === fieldToDelete.id
  );
  removeProcedureField(index);
}

function archiveProcedureField(fieldToArchive: ProcedureField) {
  const index = procedureFields.value.findIndex(
    (field) => (field.value as PF['value']).id === fieldToArchive.id
  );
  if (index !== -1) {
    (procedureFields.value[index].value as PF['value']).archived = true;
  }
}

const handleRestoreProperty = (procedureField: ProcedureField) => {
  if (procedureField.archived) {
    actionableProcedureField.value = procedureField;
    nextTick(() => {
      restoreProcedureFieldModal.value?.setModalOpen(true);
    });
  }
};

function restoreProcedureField(fieldToRestore: ProcedureField) {
  const index = procedureFields.value.findIndex(
    (field) => (field.value as PF['value']).id === fieldToRestore.id
  );
  if (index !== -1) {
    (procedureFields.value[index].value as PF['value']).archived = false;
  }
}

// Delete, Archive & Restore Procedure Field Option
const actionableProcedureFieldOption = ref<ProcedureFieldOption>();
const actionableProcedureFieldIndex = ref<number>(-1);
const actionableProcedureFieldOptionIndex = ref<number>(-1);

const deleteProcedureFieldOptionModal = ref<InstanceType<typeof DeleteProcedureFieldOptionModal>>();
const archiveProcedureFieldOptionModal =
  ref<InstanceType<typeof ArchiveProcedureFieldOptionModal>>();
const restoreProcedureFieldOptionModal =
  ref<InstanceType<typeof RestoreProcedureFieldOptionModal>>();

const handleDeleteOption = (
  procedureFieldOption: ProcedureFieldOption,
  propertyIndex: number,
  optionIndex: number
) => {
  const storeProcedureFieldOption = props.procedure.fields?.[propertyIndex]?.options?.find(
    (option) => option?.id === procedureFieldOption.id
  );
  // remove option if not saved yet
  if (!storeProcedureFieldOption) {
    const options = (procedureFields.value[propertyIndex].value as PF['value']).options;
    if (propertyIndex !== -1 && optionIndex !== -1) {
      options.splice(optionIndex, 1);
    }
    return;
  }
  actionableProcedureFieldOption.value = storeProcedureFieldOption;
  actionableProcedureFieldIndex.value = propertyIndex;
  actionableProcedureFieldOptionIndex.value = optionIndex;
  if (storeProcedureFieldOption.references > 0) {
    nextTick(() => {
      archiveProcedureFieldOptionModal.value?.setModalOpen(true);
    });
  } else {
    nextTick(() => {
      deleteProcedureFieldOptionModal.value?.setModalOpen(true);
    });
  }
};
function deleteProcedureFieldOption() {
  const options = (procedureFields.value[actionableProcedureFieldIndex.value].value as PF['value'])
    .options;
  if (
    actionableProcedureFieldIndex.value !== -1 &&
    actionableProcedureFieldOptionIndex.value !== -1
  ) {
    options.splice(actionableProcedureFieldOptionIndex.value, 1);
    actionableProcedureFieldIndex.value = -1;
    actionableProcedureFieldOptionIndex.value = -1;
  }
}

function archiveProcedureFieldOption() {
  const options = (procedureFields.value[actionableProcedureFieldIndex.value].value as PF['value'])
    .options;
  if (
    actionableProcedureFieldIndex.value !== -1 &&
    actionableProcedureFieldOptionIndex.value !== -1
  ) {
    options[actionableProcedureFieldOptionIndex.value].archived = true;
    actionableProcedureFieldIndex.value = -1;
    actionableProcedureFieldOptionIndex.value = -1;
  }
}

const handleRestoreOption = (
  procedureFieldOption: ProcedureFieldOption,
  propertyIndex: number,
  optionIndex: number
) => {
  if (procedureFieldOption.archived) {
    actionableProcedureFieldOption.value = procedureFieldOption;
    actionableProcedureFieldIndex.value = propertyIndex;
    actionableProcedureFieldOptionIndex.value = optionIndex;
    nextTick(() => {
      restoreProcedureFieldOptionModal.value?.setModalOpen(true);
    });
  }
};

function restoreProcedureFieldOption() {
  const options = (procedureFields.value[actionableProcedureFieldIndex.value].value as PF['value'])
    .options;
  if (
    actionableProcedureFieldIndex.value !== -1 &&
    actionableProcedureFieldOptionIndex.value !== -1
  ) {
    options[actionableProcedureFieldOptionIndex.value].archived = false;
    actionableProcedureFieldIndex.value = -1;
    actionableProcedureFieldOptionIndex.value = -1;
  }
}

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
              :disabled="isProcedureArchived"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.LastName"
              label="Patient Last Name "
              class="font-medium text-sm"
              :disabled="isProcedureArchived"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.DateOfBirth"
              label="Date of Birth"
              class="font-medium text-sm"
              :disabled="isProcedureArchived"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.OrderingProvider"
              label="Ordering Provider"
              class="font-medium text-sm"
              :disabled="isProcedureArchived"
            />
            <CustomCheckbox
              v-model="selectedRequiredData"
              :value="RequiredPatientData.Mrn"
              label="MRN"
              class="font-medium text-sm"
              :disabled="isProcedureArchived"
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
            <fwb-toggle
              v-model="enablePerformanceReporting"
              label="Performance Reporting"
              :disabled="isProcedureArchived"
            />
          </div>
        </AccordionDefault>
      </div>
      <hr class="border-slate-300" />
      <div v-if="props.procedure?.createdAt" class="p-4 flex flex-col gap-2">
        <div class="text-xs font-medium text-slate-500">
          Updated:
          {{
            DateTimeFormatter.formatDatetime(
              props.procedure.modifiedAt ? props.procedure.modifiedAt : props.procedure.createdAt
            )
          }}
        </div>
        <div class="text-xs font-medium text-slate-500">
          Created: {{ DateTimeFormatter.formatDatetime(props.procedure.createdAt) }}
        </div>
      </div>
    </div>

    <!-- Right pannel -->
    <div class="h-full w-full lg:flex-1 py-4 lg:px-10 px-4 overflow-y-auto custom-scroll">
      <div class="text-xl font-semibold">Procedure Fields</div>
      <div>
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
              :id="`property-${index}`"
              custom_header
              class="pb-2 lg:pb-4 my-8 border-b border-slate-300 flex-1"
              :active="editingPropertyIndexes.includes(index)"
            >
              <template #customHeader="{ open }">
                <div class="w-full flex items-center justify-between bg-slate-50 p-2 rounded-lg">
                  <div class="flex gap-2">
                    <IconDotsReorder
                      class="draggable-handle-icon cursor-grab active:cursor-grabbing"
                      @click.stop
                    />
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

                    <div class="flex items-center">
                      {{
                        (property.value as PF['value']).name
                          ? (property.value as PF['value']).name
                          : `Property ${index + 1} Details `
                      }}
                      <span class="ms-2" v-if="property.value.type">
                        <fwb-badge type="green" class="rounded-full px-2 py-0.5 text-green-600">
                          <IconNumber v-if="(property.value as PF['value']).type === 'NUMBER'" />
                          <IconText v-else-if="(property.value as PF['value']).type === 'TEXT'" />
                          <IconToggle
                            v-else-if="(property.value as PF['value']).type === 'TOGGLE'"
                          />
                          <IconList v-else-if="(property.value as PF['value']).type === 'LIST'" />
                          <span class="ml-1 text-xs font-medium">
                            {{ property.value.type }}
                          </span>
                        </fwb-badge>
                      </span>
                      <span
                        v-if="property.value.archived"
                        class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 capitalize bg-[#FCE8F3] text-[#99154B] ml-2"
                        >Archived</span
                      >
                    </div>
                  </div>
                  <div class="flex gap-2 items-center">
                    <fwb-tooltip
                      v-if="
                        !editingPropertyIndexes.includes(index) &&
                        !property.value.archived &&
                        !isProcedureArchived
                      "
                      placement="bottom"
                    >
                      <template #trigger>
                        <fwb-button
                          @click.stop="handleClickEditProperty(index)"
                          color="light"
                          pill
                          square
                          class="border-transparent hover:bg-transparent bg-transparent"
                        >
                          <IconEdit03 />
                        </fwb-button>
                      </template>
                      <template #content> Bulk edit </template>
                    </fwb-tooltip>
                    <fwb-button
                      v-if="property.value.archived"
                      @click.prevent.stop="handleRestoreProperty(property.value)"
                      color="light"
                      pill
                      square
                      class="border-transparent hover:bg-transparent bg-transparent"
                      :disabled="isProcedureArchived"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                      >
                        <path
                          d="M2 10C2 10 4.00498 7.26822 5.63384 5.63824C7.26269 4.00827 9.5136 3 12 3C16.9706 3 21 7.02944 21 12C21 16.9706 16.9706 21 12 21C7.89691 21 4.43511 18.2543 3.35177 14.5M2 10V4M2 10H8"
                          stroke="currentColor"
                          stroke-width="1.5"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                        />
                      </svg>
                    </fwb-button>
                    <fwb-button
                      v-if="!property.value.archived"
                      @click.prevent.stop="handleDeleteProperty(property.value, index)"
                      color="light"
                      pill
                      square
                      class="border-transparent hover:bg-transparent bg-transparent"
                      :disabled="isProcedureArchived"
                    >
                      <IconTrash01 height="16" width="16" />
                    </fwb-button>
                  </div>
                </div>
              </template>

              <div v-if="!editingPropertyIndexes.includes(index)" class="flex flex-col gap-4 pt-4">
                <!-- label -->
                <div class="flex sm:items-center sm:flex-row flex-col gap-2">
                  <div class="text-xs font-medium text-slate-500 sm:w-44">Label</div>
                  <div class="text-sm font-semibold text-slate-900 flex-1">
                    {{ (property.value as PF['value']).name }}
                  </div>
                </div>
                <!-- instructions -->
                <div class="flex sm:items-center sm:flex-row flex-col gap-2">
                  <div class="text-xs font-medium text-slate-500 sm:w-44">Instruction</div>
                  <div class="text-sm font-semibold text-slate-900 flex-1">
                    {{ (property.value as PF['value']).instruction }}
                  </div>
                </div>
                <!-- Property Type -->
                <div class="flex sm:items-center sm:flex-row flex-col gap-2">
                  <div class="text-xs font-medium text-slate-500 sm:w-44">Property Type</div>
                  <div class="text-sm font-semibold text-slate-900 flex-1">
                    {{ (property.value as PF['value']).type }}
                  </div>
                </div>
                <!-- Min Max Values -->
                <!-- <div v-if="(property.value as PF['value']).type === 'NUMBER'">
                  <div class="flex sm:items-center sm:flex-row flex-col gap-2">
                    <div class="text-xs font-medium text-slate-500 sm:w-44">Min. Value</div>
                    <div class="text-sm font-semibold text-slate-900 flex-1">
                      {{ property.minValue }}
                    </div>
                  </div>
                  <div class="flex sm:items-center sm:flex-row flex-col mt-4 gap-2">
                    <div class="text-xs font-medium text-slate-500 sm:w-44">Max. Value</div>
                    <div class="text-sm font-semibold text-slate-900 flex-1">
                      {{ property.maxValue }}
                    </div>
                  </div>
                </div> -->
                <!--  option list -->
                <div
                  v-if="(property.value as PF['value']).type === 'LIST'"
                  class="flex sm:flex-row flex-col gap-2"
                >
                  <div class="text-xs font-medium text-slate-500 sm:w-44">Selection Type</div>
                  <div class="text-sm font-semibold text-slate-900 flex-1">
                    <div>
                      {{
                        (property.value as PF['value']).allowMultiSelect
                          ? 'Multiple selection'
                          : 'Single selection'
                      }}
                    </div>
                    <!-- options List -->
                    <div class="p-2 mt-4 gap-1 bg-slate-50 rounded-lg">
                      <draggable
                        v-model="(property.value as PF['value']).options"
                        :item-key="Math.random().toFixed(20)"
                        tag="div"
                        v-bind="dragOptions"
                        handle=".draggable-handle-icon"
                      >
                        <template #item="{ element }">
                          <div class="flex items-center hover:bg-slate-200 p-2 rounded-lg">
                            <IconDotsReorder
                              class="mr-5 flex-shrink-0 draggable-handle-icon cursor-grab"
                            />
                            <ReadonlyCheckbox
                              v-if="
                              (property.value as PF['value']).allowMultiSelect"
                              :label="element.text"
                              class="font-medium text-sm pointer-events-none"
                              :archived="element.archived"
                            />
                            <ReadonlyRadio
                              v-else
                              :label="element.text"
                              class="font-medium text-sm pointer-events-none"
                              :archived="element.archived"
                            />
                          </div>
                        </template>
                      </draggable>
                    </div>
                  </div>
                </div>
              </div>

              <form v-else class="flex flex-col gap-4 pt-2">
                <!-- Label -->
                <div>
                  <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                    >Label</label
                  >
                  <fwb-input
                    v-model="(property.value as PF['value']).name"
                    placeholder="Write text here"
                    :class="
                    updateProcedureErrors[`procedureFields[${index}].name` as any] && isUpdateProcedureSubmit
                      ? inputErrorClasses
                      : ''
                  "
                  />
                  <span
                    v-if="
                    updateProcedureErrors[`procedureFields[${index}].name` as any] && isUpdateProcedureSubmit
                  "
                    class="text-sm text-radical-red-600"
                    >{{ updateProcedureErrors[`procedureFields[${index}].name` as any] }}</span
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
                    updateProcedureErrors[`procedureFields[${index}].type` as any] && isUpdateProcedureSubmit
                      ? inputErrorClasses
                      : ''
                  "
                  />
                  <span
                    v-if="
                    updateProcedureErrors[`procedureFields[${index}].type` as any] && isUpdateProcedureSubmit
                  "
                    class="text-sm text-radical-red-600"
                    >{{ updateProcedureErrors[`procedureFields[${index}].type` as any] }}</span
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
                  <div class="p-4 gap-1 bg-slate-50 rounded-lg my-4">
                    <draggable
                      v-if="(property.value as PF['value']).options"
                      v-model="(property.value as PF['value']).options"
                      :item-key="Math.random().toFixed(30)"
                      tag="div"
                      v-bind="dragOptions"
                      handle=".draggable-handle-icon"
                    >
                      <template #item="{ element, index: optionIndex }">
                        <div
                          @click="
                            () => {
                              if (!element.archived) toggleEditOption(index, optionIndex);
                            }
                          "
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
                              v-model="element.text"
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
                            <ReadonlyCheckbox
                              v-if="(property.value as PF['value']).allowMultiSelect"
                              :label="element.text"
                              class="font-medium text-sm my-2 pointer-events-none"
                              :archived="element.archived"
                            />

                            <ReadonlyRadio
                              v-else
                              name="radio"
                              :label="element.text"
                              class="font-medium text-sm my-2 pointer-events-none"
                              :archived="element.archived"
                            />
                            <svg
                              v-if="element.archived"
                              class="cursor-pointer block ms-auto"
                              @click.stop="handleRestoreOption(element, index, optionIndex)"
                              xmlns="http://www.w3.org/2000/svg"
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                            >
                              <path
                                d="M2 10C2 10 4.00498 7.26822 5.63384 5.63824C7.26269 4.00827 9.5136 3 12 3C16.9706 3 21 7.02944 21 12C21 16.9706 16.9706 21 12 21C7.89691 21 4.43511 18.2543 3.35177 14.5M2 10V4M2 10H8"
                                stroke="currentColor"
                                stroke-width="1.5"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                              />
                            </svg>
                            <IconTrash01
                              v-else
                              @click.stop="handleDeleteOption(element, index, optionIndex)"
                              class="ms-auto"
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
        <div v-if="!isProcedureArchived" class="mt-4">
          <button @click="addProperty" class="flex items-center text-brand-600 text-sm font-medium">
            <IconPlusCircle class="mr-0.5" /> Add Property
          </button>
        </div>
      </div>
    </div>

    <!-- Delete Procedure Field Modal -->
    <DeleteProcedureFieldModal
      v-if="actionableProcedureField"
      ref="deleteProcedureFieldModal"
      :procedureField="actionableProcedureField"
      @deleted-field="deleteProcedureField"
    />
    <!-- Archive Procedure Field Modal -->
    <ArchiveProcedureFieldModal
      v-if="actionableProcedureField"
      ref="archiveProcedureFieldModal"
      :procedureField="actionableProcedureField"
      @archived-field="archiveProcedureField"
    />
    <!-- Restore Procedure Field Modal -->
    <RestoreProcedureFieldModal
      v-if="actionableProcedureField"
      ref="restoreProcedureFieldModal"
      :procedureField="actionableProcedureField"
      @restored-field="restoreProcedureField"
    />

    <!-- Delete Procedure Field Option Modal -->
    <DeleteProcedureFieldOptionModal
      v-if="actionableProcedureFieldOption"
      ref="deleteProcedureFieldOptionModal"
      :procedureFieldOption="actionableProcedureFieldOption"
      @deleted-option="deleteProcedureFieldOption"
    />
    <!-- Archive Procedure Field Modal -->
    <ArchiveProcedureFieldOptionModal
      v-if="actionableProcedureFieldOption"
      ref="archiveProcedureFieldOptionModal"
      :procedureFieldOption="actionableProcedureFieldOption"
      @archived-option="archiveProcedureFieldOption"
    />
    <!-- Restore Procedure Field Modal -->
    <RestoreProcedureFieldOptionModal
      v-if="actionableProcedureFieldOption"
      ref="restoreProcedureFieldOptionModal"
      :procedureFieldOption="actionableProcedureFieldOption"
      @restored-option="restoreProcedureFieldOption"
    />
  </div>
</template>

<style scoped>
/* Select flowbite validition classes */
:deep(.bg-radical-red-50 select) {
  border-color: #e11d47;
  background-color: inherit;
}
</style>
