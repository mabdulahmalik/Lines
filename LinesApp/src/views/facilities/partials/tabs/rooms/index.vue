<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { FwbButton } from 'flowbite-vue';
import { IconSettings02, IconArrowNarrowDown, IconArrowNarrowUp } from '@/components/icons/index';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import AddRoomModal from '../../modal/AddRoomModal.vue';
import EditRoomModal from '../../modal/EditRoomModal.vue';
import { useModalStore } from '@/stores/modal';
import { Facility } from '@/api/__generated__/graphql';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useRouter } from 'vue-router';
import { useSidebarStore } from '@/stores/sidebar';
import { useTabStore } from '@/stores/tab';
const tabStore = useTabStore();
const sidebarStore = useSidebarStore();
const router = useRouter();
const modalStore = useModalStore();
const facilitiesStore = useFacilitiesStore();
const facilityTypesStore = useFacilityTypesStore();

const props = defineProps<{
  facility?: Facility;
}>();

const emit = defineEmits<{
  (e: 'width', val: string): void;
  (e: 'closeModal', val: boolean): void;
}>();

onMounted(() => {
  facilitiesStore.getRooms(facilitiesStore.selectedFacility.id);
});

// Sort rooms by name
const sortDirection = ref<'asc' | 'desc'>('asc');

const rooms = computed(() => {
  const sortedRooms = [...facilitiesStore.rooms].sort((a: any, b: any) => {
    const nameA = a.name.toUpperCase();
    const nameB = b.name.toUpperCase();
    if (nameA < nameB) return sortDirection.value === 'asc' ? -1 : 1;
    if (nameA > nameB) return sortDirection.value === 'asc' ? 1 : -1;
    return 0;
  });
  return sortedRooms;
});

const toggleSortDirection = () => {
  sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
};

// facility types
const facilityTypes = computed(() => facilityTypesStore.facilityTypes);
// rooms properties
const properties = computed(() => {
  const facilityType = facilityTypes.value.find(
    (facility) => facility.id === facilitiesStore.selectedFacility.typeId
  );
  return facilityType ? facilityType.properties : [];
});

// Fetch option texts by property
const getPropertyOptionsText = (propertyId: string, selectedOptionId: string) => {
  // Find the property with the matching propertyId
  const property = properties.value?.find((prop) => prop?.id === propertyId);
  if (property) {
    const selectedOption = property.options?.find((opt) => opt?.id === selectedOptionId);
    return selectedOption ? selectedOption.text : 'Option not found';
  }
  return '-';
};

//  Add Room Modal Drawer

const addRoomModalComponentRef = ref<InstanceType<typeof AddRoomModal>>();
function handleClickAddRoom() {
  addRoomModalComponentRef.value?.setModalOpen(true);
  if (!modalStore.isFacilitiesModalExpended) {
    emit('width', 'full');
    modalStore.isFacilitiesModalExpended = true;
    modalStore.isFacilitiesModalAutoExpended = true;
  }
}

function AddRoomModalClosed() {
  if (modalStore.isFacilitiesModalAutoExpended) {
    emit('width', '5xl');
    modalStore.isFacilitiesModalExpended = false;
    modalStore.isFacilitiesModalAutoExpended = false;
  }
}

const activeRoomIndex = ref<number>();
const activeRoom = ref<any>({
  facilityId: null,
  id: null,
  name: '',
  properties: [],
});

const editRoomModalComponentRef = ref<InstanceType<typeof EditRoomModal>>();
function handleClickEditRoom(room: any, index: number) {
  activeRoom.value = room;
  activeRoomIndex.value = index;
  editRoomModalComponentRef.value?.setModalOpen(true);
  if (!modalStore.isFacilitiesModalExpended) {
    emit('width', 'full');
    modalStore.isFacilitiesModalExpended = true;
    modalStore.isFacilitiesModalAutoExpended = true;
  }
}

function EditRoomModalClosed() {
  if (modalStore.isFacilitiesModalAutoExpended) {
    emit('width', '5xl');
    modalStore.isFacilitiesModalExpended = false;
    modalStore.isFacilitiesModalAutoExpended = false;
  }
}

// navigate To Settings
const navigateToSettings = () => {
  emit('closeModal', true);
  router.push({ path: '/settings/facilities' });
  sidebarStore.page = 'settings';
  sidebarStore.selected = 'Facilities';
  tabStore.setFacilitiesActiveTab(1);
};
</script>

<template>
  <!-- Rooms -->
  <div>
    <div>
      <div class="font-semibold text-lg">Rooms</div>
      <div class="font-medium text-sm pt-1">
        A Room is a physical space within a facility, organized by properties like floor or wing.
        This helps to manage and identify spaces efficiently for operations and reporting.
      </div>
    </div>

    <!-- Settings & Add room Button -->
    <div class="flex justify-end gap-4 mt-4 py-[14px]">
      <fwb-button
        @click="navigateToSettings"
        color="light"
        pill
        :disabled="facility?.archived ?? false"
        class="font-semibold inline-flex"
      >
        <template #prefix>
          <IconSettings02 />
        </template>
        Go to settings
      </fwb-button>
      <fwb-button
        @click="handleClickAddRoom"
        class="bg-primary-600 hover:bg-brand-600 font-semibold inline-flex"
        pill
        :disabled="facility?.archived ?? false"
      >
        Add Room
      </fwb-button>
    </div>
    <!-- Table -->
    <div class="overflow-x-auto">
      <base-table>
        <t-head class="uppercase">
          <t-r>
            <t-h class="lg:min-w-36 min-w-32 text-nowrap sticky left-0 bg-white z-10">
              <span
                @click="toggleSortDirection"
                class="text-slate-800 inline-flex items-center gap-1 cursor-pointer"
              >
                <span>Name</span>
                <span
                  class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                >
                  <template v-if="sortDirection === 'asc'">
                    <IconArrowNarrowUp />
                  </template>
                  <template v-else>
                    <IconArrowNarrowDown />
                  </template>
                </span>
              </span>
            </t-h>
            <t-h
              v-for="(room, indx) in properties"
              :key="indx"
              class="min-w-36 bg-white text-nowrap"
            >
              {{ room?.name }}
            </t-h>
          </t-r>
        </t-head>
        <t-body v-if="rooms.length > 0">
          <t-r v-for="(room, index) in rooms" :key="index" class="group/row hover:bg-slate-50">
            <t-d
              class="font-semibold text-brand-600 sticky left-0 z-10 bg-white group-hover/row:bg-slate-50"
            >
              <a @click.prevent="handleClickEditRoom(room, index)" href="#">{{ room.name }} </a>
            </t-d>
            <template v-for="colIndex in properties?.length" :key="colIndex">
              <t-d class="text-nowrap">
                <template v-if="room.properties">
                  {{
                    getPropertyOptionsText(
                      room.properties[colIndex - 1]?.propertyId,
                      room.properties[colIndex - 1]?.value
                    )
                  }}
                </template>
                <template v-else>-</template>
              </t-d>
            </template>
          </t-r>
        </t-body>
        <t-body v-else>
          <t-r>
            <t-d colspan="5" class="text-center">No Rooms to Display</t-d>
          </t-r>
        </t-body>
      </base-table>
    </div>

    <!-- Add Execution Modal Drawer -->
    <AddRoomModal
      ref="addRoomModalComponentRef"
      :facility="props.facility"
      @close="AddRoomModalClosed"
    />

    <!-- Edit Room Modal Drawer -->
    <EditRoomModal
      ref="editRoomModalComponentRef"
      :room="activeRoom"
      :facility="props.facility"
      @close="EditRoomModalClosed"
    />
  </div>
</template>
