<script setup lang="ts">
import { ref, onUnmounted, watch, onMounted } from 'vue';
import { FwbButton } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import ViewRoomPropertyModalBody from './ViewRoomPropertyModalBody.vue';
import {
  IconExpand01,
  IconDotHorizontal,
  IconTrash01,
  IconArrowLeft,
  IconShrink,
} from '@/components/icons/index';
import { useModalStore } from '@/stores/modal';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import DrawerHeaderEditName from '@/views/common/DrawerHeaderEditName.vue';

const props = defineProps<{
  roomProperty: any;
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
    @close="facilityTypesStore.clearSelectedRoomProperty()"

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
        <div class="hidden lg:flex items-center gap-2 lg:gap-4">
          <!-- dropdown -->
          <Dropdown align-to-end class="rounded-lg">
            <template #trigger>
              <fwb-button color="light" pill square>
                <IconDotHorizontal />
              </fwb-button>
            </template>
            <DropdownMenu>
              <DropdownItem class="!text-radical-red-700">
                <IconTrash01 class="mr-2" />
                Delete/ Archive
              </DropdownItem>
            </DropdownMenu>
          </Dropdown>

          <!-- Expand -->
          <fwb-button v-if="!fullWidth" @click="setModalWidth('full')" color="light" pill square>
            <IconExpand01 />
          </fwb-button>
          <!-- Shrink -->
          <fwb-button v-if="fullWidth" @click="setModalWidth('5xl')" color="light" pill square>
            <IconShrink />
          </fwb-button>
        </div>
      </div>
    </template>
    <template #body class="bg-red-200">
      <ViewRoomPropertyModalBody
        ref="ViewRoomPropertyModalBodyRef"
        :name="roomPropertyName"
        :roomProperty="props.roomProperty"
        @close="setModalOpen(false)"
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
