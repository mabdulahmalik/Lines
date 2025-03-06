<script setup lang="ts">
import { ref, onUnmounted, watch, onMounted } from 'vue';
import { FwbButton } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import ViewRoomPropertyModalBody from './ViewRoomPropertyModalBody.vue';
import {
  IconArrowLeft,
  IconExpandWindows,
  IconShrinkWindows,
  IconShowSidebar,
  IconHideSidebar,
} from '@/components/icons/index';
import { useModalStore } from '@/stores/modal';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import DrawerHeaderEditName from '@/views/common/DrawerHeaderEditName.vue';
import { FacilityRoomProperty } from '@/api/__generated__/graphql';

const props = defineProps<{
  roomProperty: FacilityRoomProperty;
  facilityName: string;
}>();

// Emits
const emit = defineEmits<{
  (e: 'width', val: string): void;
  (e: 'property-modified', val: FacilityRoomProperty): void;
}>();

const facilityTypesStore = useFacilityTypesStore();
const modalStore = useModalStore();

const modalViewRoomPropertyRef = ref<InstanceType<typeof ModalDrawer>>();

const roomPropertyName = ref('');

onMounted(() => {
  roomPropertyName.value = props.roomProperty.name;
});
watch(
  () => props.roomProperty.name,
  (newValue) => {
    roomPropertyName.value = newValue;
  }
);
function changeName(val: string) {
  roomPropertyName.value = val;
}

const setModalOpen = (value: boolean): void => {
  modalViewRoomPropertyRef.value?.setModalOpen(value);
};

function closeViewRoomModal() {
  modalViewRoomPropertyRef.value?.setModalOpen(false);
  facilityTypesStore.clearSelectedRoomProperty();
}

// submit data
const ViewRoomPropertyModalBodyRef = ref<InstanceType<typeof ViewRoomPropertyModalBody>>();

function handelModifyRoomProperty() {
  ViewRoomPropertyModalBodyRef.value?.submittedData();
}

function propertyModified(rp: FacilityRoomProperty){
  emit('property-modified', rp)
}

// Room Properties Modal width
const fullWidth = ref(false);
const modalWidth = ref('5xl');
const setModalWidth = (val: string) => {
  modalWidth.value = val;
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isModalDrawerExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isModalDrawerExpended = false;
  }
};

function handleModalClosed() {
  facilityTypesStore.clearSelectedRoomProperty()
  // reset facility modal width if auto expended
  if (modalStore.isFacilityTypeModalAutoExpended) {
    emit('width', '5xl');
    modalStore.isFacilityTypeModalExpended = false;
    modalStore.isFacilityTypeModalAutoExpended = false;
  }
}
defineExpose({
  setModalOpen,
});
onUnmounted(() => {
  modalStore.isModalDrawerExpended = false;
});


</script>
<template>
  <ModalDrawer
    ref="modalViewRoomPropertyRef"
    :z_index="46"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    @close="handleModalClosed"
  >
    <template #header>
      <div class="p-4 lg:px-6 flex justify-between min-h-[72px]">
        <div class="flex items-center">
          <!-- Mobile close button -->
          <fwb-button
            @click="closeViewRoomModal"
            color="light"
            pill
            square
            size="lg"
            class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
          >
            <IconArrowLeft width="24" height="24" class="text-[#334155]" />
            <span class="sr-only">Close modal</span>
          </fwb-button>

          <DrawerHeaderEditName :name="roomPropertyName" @name-updated="changeName" />
        </div>
        <!-- Icon Buttons Desktop only -->
        <div class="hidden lg:flex items-center gap-2">
          <!-- Expand -->
          <fwb-button
            v-if="!fullWidth"
            @click="setModalWidth('full')"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconExpandWindows width="22" height="22" />
          </fwb-button>
          <!-- Shrink -->

          <fwb-button
            v-if="fullWidth"
            @click="setModalWidth('5xl')"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconShrinkWindows width="22" height="22" />
          </fwb-button>
          <!-- Hide Sidebar -->
          <fwb-button
            v-if="modalStore.isRoomPropertySidebarOpen"
            @click="modalStore.isRoomPropertySidebarOpen = false"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconHideSidebar />
          </fwb-button>
          <!-- Show Sidebar -->
          <fwb-button
            v-else
            @click="modalStore.isRoomPropertySidebarOpen = true"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconShowSidebar />
          </fwb-button>
        </div>
      </div>
    </template>
    <template #body class="bg-red-200">
      <ViewRoomPropertyModalBody
        ref="ViewRoomPropertyModalBodyRef"
        :facility-name="props.facilityName"
        :name="roomPropertyName"
        :roomProperty="props.roomProperty"
        @close="setModalOpen(false)"
        @property-modified="propertyModified"
      />
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <div class="flex items-center gap-4 lg:gap-6">
          <fwb-button @click="closeViewRoomModal" color="light" pill> Cancel</fwb-button>
          <fwb-button
            @click="handelModifyRoomProperty"
            class="bg-primary-600 hover:bg-brand-600"
            pill
            >Save</fwb-button
          >
        </div>
      </div>
    </template>
  </ModalDrawer>
</template>
