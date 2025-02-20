<script setup lang="ts">
import { ref, watch } from 'vue';
import { useFacilitiesStore } from '@/stores/data/facilities';

const props = defineProps<{ facilityId: string }>();

const roomCount = ref<number | null>(null);

const facilitiesStore = useFacilitiesStore();

const fetchRoomCount = async (facilityId: string) => {
  roomCount.value = await facilitiesStore.getRoomCountForFacility(facilityId);
};

watch(() => props.facilityId, async (newFacilityId) => {
  if (newFacilityId) {
    roomCount.value = null;
    await fetchRoomCount(newFacilityId);
  }
}, { immediate: true });
</script>
<template>
  <span v-if="roomCount !== null">{{ roomCount }}</span>
  <span v-else>Loading...</span>
</template>
