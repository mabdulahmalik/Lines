<script setup lang="ts">
import { FwbButton, FwbTooltip, FwbInput } from 'flowbite-vue';
import { ref, computed, watch } from 'vue';
import { useModalStore } from '@/stores/modal';
import {
  IconExpandWindows,
  IconShrinkWindows,
  IconShowSidebar,
  IconHideSidebar,
  IconArrowLeft,
  IconDotHorizontal,
  IconTrash01,
  IconEdit05,
  IconX,
} from '@/components/icons/index';
import { MedicalRecord } from '@/api/__generated__/graphql';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import Modal from '@/components/modal/Modal.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';

const props = defineProps<{
  record: MedicalRecord;
}>();

const modalStore = useModalStore();
const medicalRecordsStore = useMedicalRecordsStore();
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const { isRecordLoading: isLoading } = useLoaders();

const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'width', val: string): void;
  (e: 'assist'): void;
}>();

const fullWidth = ref(false);
const setModalWidth = (val: string) => {
  emit('width', val);
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isRecordModalExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isRecordModalExpended = false;
  }
};
const closeModal = () => {
  emit('close');
};

// Change Number Modal

const editNumberModalRef = ref<InstanceType<typeof Modal>>();
const currentNumber = ref(props.record.number || '');
const existingRecordNumbers = computed(() => {
  return medicalRecordsStore.medicalRecords.map((num) => num.number?.toLowerCase());
});

const schemaEditNumber = yup.object({
  numberEditVal: yup
    .string()
    .required('This is a required field')
    .trim()
    .test('unique-number', 'Medical record number must be unique', (value) => {
      if (!value) return true;
      const filteredRecordsNumbers = existingRecordNumbers.value.filter(
        (num) => num !== currentNumber.value?.toLowerCase()
      );
      return !filteredRecordsNumbers.includes(value.toLowerCase());
    }),
});
const { handleSubmit: handleSubmitEditNumber } = useForm({
  validationSchema: schemaEditNumber,
  validateOnMount: false,
});
const { value: numberEditVal, errorMessage: numberEditError } = useField<string>('numberEditVal');

watch(
  () => props.record.number,
  (newValue) => {
    currentNumber.value = newValue || '';
  },
  { immediate: true }
);

const hadleChangeNumber = () => {
  if (props.record.id) {
    numberEditVal.value = props.record.number!;
    editNumberModalRef.value?.setModalOpen(true);
  }
};

const handleEditNumber = () => {
  handleSubmitEditNumber(async () => {
    const updatedNumber = {
      id: props.record.id,
      number: numberEditVal.value,
    };
    medicalRecordsStore.renumberMedicalRecord(updatedNumber);
    editNumberModalRef.value?.setModalOpen(false);
  })();
};

const handleCancelEditNumber = () => {
  editNumberModalRef.value?.setModalOpen(false);
};

// Delete Modal

const deleteRecordModalRef = ref<InstanceType<typeof Modal>>();
const hadleDeleteRecord = () => {
  if (props.record.id) {
    deleteRecordModalRef.value?.setModalOpen(true);
  }
};
const closeDeleteRecordModal = () => {
  deleteRecordModalRef.value?.setModalOpen(false);
};

// Validation
const schemaDeleteMR = yup.object({
  medicalRecordForDelete: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitDeleteMR } = useForm({
  validationSchema: schemaDeleteMR,
});
const {
  value: medicalRecordForDelete,
  errorMessage: mrNumberError,
  resetField: resetmrNumber,
} = useField<string>('medicalRecordForDelete');

function handleDeleteMedicalRecord() {
  handleSubmitDeleteMR(async () => {
    medicalRecordsStore.deleteMedicalRecord({ id: props.record.id });
    closeDeleteRecordModal();
    resetmrNumber();
    emit('close');
    medicalRecordsStore.clearSelectedMedicalRecord();
  })();
}
</script>

<template>
<div>
    <!-- loading -->
    <div v-if="isLoading" class="flex items-center justify-between gap-x-2 py-2 px-4 lg:px-6 lg:gap-x-4 flex-wrap">
    <div class="flex items-center gap-2  w-full lg:w-auto lg:justify-none min-h-10 justify-start">
    <!-- Mobile close button -->
    <fwb-button
    @click="closeModal"
    color="light"
    pill
    square
    size="lg"
    class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
    >
    <IconArrowLeft width="24" height="24" class="text-[#334155] -ml-1" />
    </fwb-button>
    <SkeletonItem backgroundColor="#CBD5E1" class=" sm:w-56 w-40 h-4 rounded-3xl" />
    </div>
  <hr class="lg:hidden h-[1px] border-t border-slate-300 pt-2 mt-1.5 -mx-4 w-[calc(100%+32px)]" />
    <div class="flex items-center  w-full lg:w-auto justify-end flex-1 min-h-10 gap-4">
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    </div>
  </div>

  <div v-else class="flex items-center justify-between gap-x-2 py-2 px-4 lg:px-6 lg:gap-x-4 flex-wrap">
    <!-- Title -->
    <div class="flex items-center gap-2 justify-between w-full lg:w-auto lg:justify-none min-h-10">
      <div class="flex items-center">
        <!-- Mobile close button -->
        <fwb-button
          @click="closeModal"
          color="light"
          pill
          square
          size="lg"
          class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
        >
          <IconArrowLeft width="24" height="24" class="text-[#334155] -ml-1" />
          <span class="sr-only">Close modal</span>
        </fwb-button>
        <div class="flex items-center gap-3">
          <h2 class="text-lg lg:text-2xl font-semibold">
            {{ props.record.number }}
          </h2>
        </div>
      </div>
    </div>
    <hr class="lg:hidden h-[1px] border-t border-slate-300 pt-2 mt-1.5 -mx-4 w-[calc(100%+32px)]" />

    <div class="flex items-center gap-3 lg:gap-4 w-full lg:w-auto justify-end">
      <!-- Icon Buttons -->
      <div class="flex items-center gap-1 lg:gap-2">
        <!-- More dropdown  -->
        <Dropdown align-to-end class="rounded-lg" close-inside>
          <template #trigger>
            <fwb-button color="light" pill square>
              <IconDotHorizontal />
            </fwb-button>
          </template>
          <DropdownMenu class="min-w-64">
            <DropdownItem @click="hadleChangeNumber">
              <IconEdit05 class="mr-2" />
              Change Record Number
            </DropdownItem>
            <DropdownItem @click="hadleDeleteRecord" class="!text-radical-red-700">
              <IconTrash01 class="mr-2" />
              Delete Medical Record
            </DropdownItem>
          </DropdownMenu>
        </Dropdown>

        <!-- Expand -->
        <fwb-tooltip v-if="!fullWidth" placement="bottom" class="hidden lg:block">
          <template #trigger>
            <fwb-button
              @click="setModalWidth('full')"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconExpandWindows width="24" height="24" />
            </fwb-button>
          </template>
          <template #content> Expand windows </template>
        </fwb-tooltip>

        <!-- Shrink -->
        <fwb-tooltip v-if="fullWidth" placement="bottom" class="hidden lg:block">
          <template #trigger>
            <fwb-button
              @click="setModalWidth('7xl')"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconShrinkWindows width="24" height="24" />
            </fwb-button>
          </template>
          <template #content> Shrink windows </template>
        </fwb-tooltip>

        <!-- Hide Sidebar -->
        <fwb-tooltip placement="bottom" v-if="modalStore.isRecordSidebarOpen">
          <template #trigger>
            <fwb-button
              @click="modalStore.isRecordSidebarOpen = false"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconHideSidebar />
            </fwb-button>
          </template>
          <template #content> Hide Sidebar </template>
        </fwb-tooltip>

        <!-- Show Sidebar -->
        <fwb-tooltip placement="bottom" v-else>
          <template #trigger>
            <fwb-button
              @click="modalStore.isRecordSidebarOpen = true"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconShowSidebar />
            </fwb-button>
          </template>
          <template #content> Show Sidebar </template>
        </fwb-tooltip>
      </div>
    </div>

    <!-- Change Number modal -->
    <teleport to="body">
      <Modal ref="editNumberModalRef" title=" Change Record Number" :z_index="55">
        <template #body>
          <div class="flex flex-col gap-3 p-6">
            <div>
              <label class="mb-1 block text-sm font-medium text-slate-900 dark:text-white"
                >Number</label
              >
              <fwb-input
                v-model.trim="numberEditVal"
                placeholder="enter Number ..."
                :class="numberEditError ? inputErrorClasses : ''"
              />
              <span v-if="numberEditError" class="text-sm text-radical-red-600">{{
                numberEditError
              }}</span>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-2 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="handleCancelEditNumber" color="light" pill> Cancel</fwb-button>
            <fwb-button @click="handleEditNumber" class="bg-primary-600 hover:bg-brand-600" pill>
              Save & close
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>

    <!-- Delete Record Modal -->
    <teleport to="body">
      <Modal ref="deleteRecordModalRef" title="Delete Medical Record" size="lg" :z_index="55">
        <template #body>
          <div class="p-4 lg:p-6">
            <div class="flex flex-col gap-4">
              <div class="text-sm font-medium">
                Deleting this medical record will permanently remove it from your Account.
              </div>
              <div class="text-sm font-bold">I understand that</div>
              <div class="text-radical-red-500 flex flex-col gap-2 text-xs font-medium">
                <div class="flex sm:items-center gap-2">
                  <IconX class="flex-shrink-0" /> This medical record will no longer be accessible
                </div>
                <div class="flex sm:items-center gap-2">
                  <IconX class="flex-shrink-0" /> This action cannot be undone
                </div>
              </div>
              <div>
                <label class="mb-2 block text-sm font-medium"
                  >Confirm by typing
                  <span class="text-purple-800 px-0.5">{{ props.record.number }}</span>
                  below.</label
                >
                <fwb-input
                  v-model.trim="medicalRecordForDelete"
                  :class="mrNumberError ? inputErrorClasses : ''"
                  placeholder="Medical Record Number in here"
                />
                <span v-if="mrNumberError" class="text-sm text-radical-red-600">{{
                  mrNumberError
                }}</span>
              </div>
            </div>
          </div>
        </template>
        <template #footer>
          <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
            <fwb-button @click="closeDeleteRecordModal" color="light" pill> Cancel</fwb-button>
            <fwb-button
              @click.prevent="handleDeleteMedicalRecord"
              class="bg-radical-red-700 hover:bg-radical-red-700 focus:ring-0"
              pill
              :disabled="medicalRecordForDelete !== props.record.number"
            >
              Delete Medical Record
            </fwb-button>
          </div>
        </template>
      </Modal>
    </teleport>
  </div>
</div>
</template>
