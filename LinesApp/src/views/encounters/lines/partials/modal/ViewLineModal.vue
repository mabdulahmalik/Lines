<script setup lang="ts">
import { ref, watch } from 'vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import ViewLineModalHeader from './ViewLineModalHeader.vue';
import { useModalStore } from '@/stores/modal';
import {  Line } from '@/api/__generated__/graphql';
import LinePaginationBar from '../LinePaginationBar.vue';
import ViewModalBodyLine from './ViewModalBodyLine.vue';
import { FwbButton } from 'flowbite-vue';
import { useLinesStore } from '@/stores/data/encounters/lines';

const props = defineProps<{
  line: Line;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
}>();

const linesStore = useLinesStore();
const modalStore = useModalStore();

const modalViewLineRef = ref<InstanceType<typeof ModalDrawer>>();
const modalViewLineBodyRef = ref<InstanceType<typeof ViewModalBodyLine>>();
const modalWidth = ref('7xl');
const setModalOpen = (value: boolean): void => {
  modalViewLineRef.value?.setModalOpen(value);
};
const closeModal = () => {
  modalViewLineRef.value?.setModalOpen(false);
};

function setModalWidth(val: string) {
  modalWidth.value = val;
}

const lineModalClosed = () => {
  linesStore.clearSelectedLine();
  modalStore.isLineModalExpended = false;
  setModalWidth('7xl');
  emit('close');
};

// line Name Edited
const lineName = ref<string>(props.line.name || '');
const lineNameEdited = (newName: string) => {
  lineName.value = newName;
};
watch(
  () => props.line.name,
  (newValue) => {
    lineName.value = newValue ?? '';
  }
);

// Edit mode

const isNameEditMode=ref(false);
const isMRLocationEditMode=ref(false);
const handleEditModeName = (val: boolean) => {
  isNameEditMode.value=val;
};
const handleEditModeChange = (val: boolean) => {
  isMRLocationEditMode.value=val;
};

const isLineNameDirty=ref(false);
const unsavedDetails=ref(false);
const handleLineNameDirty = (val: boolean) => {
  isLineNameDirty.value=val;
};

const handleUnsavedDetails = (val: boolean) => {
  unsavedDetails.value=val;
};

// Submit Line data on save
const handleSaveLinesRevision = () => {
  modalViewLineBodyRef.value?.saveLinesRevision();
};

defineExpose({
  setModalOpen,
});
</script>
<template>
  <ModalDrawer
    ref="modalViewLineRef"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    left_close_inside
    :z_index="46"
    @close="lineModalClosed"
  >
    <template #sidebar>
      <LinePaginationBar :line="props.line" class="hidden lg:block" />
    </template>

    <template #header>
      <ViewLineModalHeader
        :line="props.line"
        @close="closeModal"
        @width="setModalWidth"
        @lineName="lineNameEdited"
        @isLineNameDirty="handleLineNameDirty"
        @isEditModeLineName="handleEditModeName"
      />
    </template>
    <template #body>
      <ViewModalBodyLine
        ref="modalViewLineBodyRef"
        :line="props.line"
        :lineName="lineName"
        :isLineNameDirty="isLineNameDirty"
        @width="setModalWidth"
        @close="setModalOpen(false)"
        class="lg:mb-0 mb-[46px]"
        @unsaved-details="handleUnsavedDetails" 
        @isEditModeMRLocation="handleEditModeChange"
      />
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="handleSaveLinesRevision" class="bg-primary-600 hover:bg-brand-600" pill
          :disabled="!isLineNameDirty && !unsavedDetails || isNameEditMode || isMRLocationEditMode"
          >Save</fwb-button
        >
      </div>
    </template>
  </ModalDrawer>
</template>
