<script setup lang="ts">
import { ref } from 'vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import ViewRecordModalHeader from './ViewRecordModalHeader.vue';
import ViewRecordModalBody from './ViewRecordModalBody.vue';
import { useModalStore } from '@/stores/modal';
import { MedicalRecord } from '@/api/__generated__/graphql';
import RecordPaginationBar from '../RecordPaginationBar.vue';
import { useMedicalRecordsStore } from '@/stores/data/encounters/medicalRecords';
import { FwbButton } from 'flowbite-vue';

const props = defineProps<{
  record: MedicalRecord;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
}>();

const medicalRecordsStore = useMedicalRecordsStore();
const modalStore = useModalStore();

const modalViewRecordRef = ref<InstanceType<typeof ModalDrawer>>();
const modalViewRecordBodyRef = ref<InstanceType<typeof ViewRecordModalBody>>();
const modalWidth = ref('7xl');

const setModalOpen = (value: boolean): void => {
  modalViewRecordRef.value?.setModalOpen(value);
};
function closeModal() {
  modalViewRecordRef.value?.setModalOpen(false);
}

function setModalWidth(val: string) {
  modalWidth.value = val;
}

function recordModalClosed() {
  medicalRecordsStore.clearSelectedMedicalRecord();
  modalStore.isRecordModalExpended = false;
  setModalWidth('7xl');
  emit('close');
}

// Modify record 
const isMREditMode=ref(false);
const handleEditChange = (val: boolean) => {
  isMREditMode.value=val;
};


const handleSaveMedicalRecord = () => {
  modalViewRecordBodyRef.value?.modifyMedicalRecord();
};

defineExpose({
  setModalOpen,
});
</script>
<template>
  <ModalDrawer
    ref="modalViewRecordRef"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    :z_index="46"
    left_close_inside
    @close="recordModalClosed"
  >
    <template #sidebar>
      <RecordPaginationBar :record="props.record" class="hidden lg:block" />
    </template>

    <template #header>
      <ViewRecordModalHeader :record="props.record" @close="closeModal" @width="setModalWidth" />
    </template>
    <template #body>
      <ViewRecordModalBody
        ref="modalViewRecordBodyRef"
        :record="props.record"
        @width="setModalWidth"
        @close="setModalOpen(false)"
        @isEditModeMedicalRecord="handleEditChange" 
      />
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="handleSaveMedicalRecord" class="bg-primary-600 hover:bg-brand-600" pill
        :disabled="isMREditMode"
          >Save</fwb-button
        >
      </div>
    </template>
  </ModalDrawer>
</template>
