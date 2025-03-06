<script setup lang="ts">
import { FacilityRoomProperty, FacilityType, ModifyFacilityTypeCmd } from '@/api/__generated__/graphql';
import { computed, ref } from 'vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import {
  IconDotsReorder,
  IconPlusCircle,
  IconPencilEdit02,
  IconArrowNarrowDown,
} from '@/components/icons/index';
import draggable from 'vuedraggable';
import DateTimeFormatter from '@/utils/dateTimeFormatter';
import { FwbButton, FwbToggle } from 'flowbite-vue';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { BaseTable, THead, TR, TD, TH, TBody } from '@/components/table/index';
import { useModalStore } from '@/stores/modal';
import ViewRoomPropertyModal from './ViewRoomPropertyModal.vue';
import AddRoomPropertyModal from './AddRoomPropertyModal.vue';

// Props
const props = defineProps<{
  facilityType: FacilityType;
  facilityName: string;
}>();

// Emits
const emit = defineEmits<{
  (e: 'width', val: string): void;
}>();

// Stores
const facilityTypesStore = useFacilityTypesStore();
const modalStore = useModalStore();

// Is FacilityType active
const isFacilityTypeActive = ref<boolean>(props.facilityType.isActive || false);

const roomProperties = ref(props.facilityType.properties);

// Add room properties
const addRoomPropertyModalComRef = ref<InstanceType<typeof AddRoomPropertyModal>>();
function openAddRoomPropertyModal() {
  addRoomPropertyModalComRef.value?.setModalOpen(true);
  if (!modalStore.isFacilityTypeModalExpended) {
    emit('width', 'full');
    modalStore.isFacilityTypeModalExpended = true;
    modalStore.isFacilityTypeModalAutoExpended = true;
  }
}

function addRoomProperty(rp: FacilityRoomProperty){
  console.log('added rp ready', rp, roomProperties.value)
  roomProperties.value = [...(roomProperties.value ?? []), rp];
}


// Edit/View room properties
const viewRoomPropertyModalComRef = ref<InstanceType<typeof ViewRoomPropertyModal>>();
function openViewRoomModal(item: FacilityRoomProperty) {
  facilityTypesStore.selectedRoomProperty = item;
  viewRoomPropertyModalComRef.value?.setModalOpen(true);
  if (!modalStore.isFacilityTypeModalExpended) {
    emit('width', 'full');
    modalStore.isFacilityTypeModalExpended = true;
    modalStore.isFacilityTypeModalAutoExpended = true;
  }
}

function modifyRoomProperty(updatedRp: FacilityRoomProperty) {
  if (!roomProperties.value) return;

  // Create a new array reference
  roomProperties.value = roomProperties.value.map((rp) =>
    rp?.id === updatedRp.id ? { ...updatedRp } : rp
  );
}


// Save Facility type
function handleSaveFacilityType() {
  // modifyFacilityType mutation
  const updatedFT : ModifyFacilityTypeCmd = {
    active: isFacilityTypeActive.value,
    id: props.facilityType.id,
    name: props.facilityName,
    properties: roomProperties.value
  }
  console.log('save facilty body', updatedFT);
  facilityTypesStore.modifyFacilityType(updatedFT);
}


// Set FacilityType modal width
function setModalWidth(val: string) {
  emit('width', val);
}


// drag & drop
const dragOptions = computed(() => {
  return {
    animation: 200,
    group: 'table-items',
    disabled: false,
    ghostClass: 'shadow',
  };
});

// Expose function
defineExpose({
  handleSaveFacilityType,
});
</script>

<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Left panel -->
    <div class="h-full w-full lg:flex-1 lg:py-8 py-4 lg:px-10 px-4 overflow-y-auto custom-scroll">
      <div class="flex justify-between items-start gap-5">
        <div class="flex-1">
          <div class="text-xl font-semibold">Room Properties</div>
          <div class="text-sm font-medium text-slate-500">
            Room Properties are leveraged by Facility Types to describe and define the
            organizational structure of your facilities. A Room Property can help to describe
            different aspects of a room within a facility, such as where it's located, what it's
            used for, and more.
          </div>
        </div>
        <!-- Add property -->
        <div class="pt-2">
          <fwb-button
            @click="openAddRoomPropertyModal"
            color="light"
            pill
            class="font-semibold hover:bg-white focus:ring-0 text-brand-600 border-brand-600"
          >
            <template #prefix>
              <IconPlusCircle />
            </template>
            Add property
          </fwb-button>
        </div>
      </div>
      <div class="pt-8">
        <div class="mb-4 lg:mb-10">
          <base-table class="border rounded-lg">
            <t-head class="uppercase">
              <t-r class="hover:bg-slate-50">
                <t-h class="w-6"></t-h>
                <t-h class="w-16">
                  <div class="text-slate-800 flex items-center gap-2">
                    <span>Order </span>
                    <IconArrowNarrowDown />
                  </div>
                </t-h>
                <t-h>Name</t-h>
                <t-h>No OF Values</t-h>
                <t-h class="text-right">Actions</t-h>
              </t-r>
            </t-head>

            <draggable
              v-if="roomProperties && roomProperties.length > 0"
              v-model="roomProperties"
              item-key="id"
              tag="tbody"
              v-bind="dragOptions"
              handle=".draggable-handle-icon"
              @end=""
            >
              <template #item="{ element: item, index }">
                <t-r class="hover:bg-slate-50">
                  <t-d class="w-6">
                    <IconDotsReorder
                      class="cursor-grab active:cursor-grabbing draggable-handle-icon"
                  /></t-d>
                  <t-d>{{ index + 1 }}</t-d>
                  <t-d class="text-brand-600 font-semibold">
                    <a href="#" @click="openViewRoomModal(item)">
                      {{ item.name }}
                    </a>
                  </t-d>
                  <t-d class="text-brand-600 font-semibold">{{ item.options?.length }}</t-d>
                  <t-d class="text-slate-900 text-right">
                    <div class="flex items-center justify-end">
                      <fwb-button
                        color="light"
                        pill
                        square
                        class="border-transparent bg-transparent"
                      >
                        <IconPencilEdit02 />
                      </fwb-button>
                    </div>
                  </t-d>
                </t-r>
              </template>
            </draggable>
            <t-body v-else>
              <t-r>
                <t-d colspan="5" class="text-center">No Room Properties to Display</t-d>
              </t-r>
            </t-body>
          </base-table>
        </div>
      </div>
    </div>

    <!-- Sidebar -->
    <div
      v-if="modalStore.isFacilityTypeSidebarOpen"
      class="h-auto min-h-full lg:h-full w-full lg:w-80 lg:border-l border-slate-300 overflow-y-auto custom-scroll py-4"
    >
      <div class="px-4">
        <AccordionDefault id="1" active class="pb-2 lg:pb-4 border-slate-300">
          <template #header>
            <div class="w-full flex items-center justify-between">
              <div>State</div>
            </div>
          </template>
          <div class="flex justify-between py-3">
            <div class="text-xs font-medium text-slate-500">Active</div>
            <FwbToggle v-model="isFacilityTypeActive" color="purple" />
          </div>
        </AccordionDefault>
      </div>
      <hr class="border-slate-300" />
      <div v-if="props.facilityType?.createdAt" class="p-4 flex flex-col gap-2">
        <div class="text-xs font-medium text-slate-500">
          Updated:
          {{
            DateTimeFormatter.formatDatetime(
              props.facilityType.modifiedAt
                ? props.facilityType.modifiedAt
                : props.facilityType.createdAt
            )
          }}
        </div>
        <div class="text-xs font-medium text-slate-500">
          Created: {{ DateTimeFormatter.formatDatetime(props.facilityType.createdAt) }}
        </div>
      </div>
    </div>
  </div>

  <!-- View room property drawer -->
  <ViewRoomPropertyModal
    ref="viewRoomPropertyModalComRef"
    :facility-name="props.facilityName"
    :roomProperty="facilityTypesStore.selectedRoomProperty"
    @width="setModalWidth"
    @property-modified="modifyRoomProperty"
  />
  <AddRoomPropertyModal
    :facility-type="facilityTypesStore.selectedFacilityType"
    ref="addRoomPropertyModalComRef"
    @width="setModalWidth"
    @property-added="addRoomProperty"
  />
</template>
