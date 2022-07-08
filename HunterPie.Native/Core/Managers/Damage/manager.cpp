#include "../../../pch.h"
#include "manager.h"

using namespace HunterPie::Core::Damage;

DamageTrackManager* DamageTrackManager::m_Instance;
DamageTrackManager::DamageTrackManager() {}
DamageTrackManager DamageTrackManager::operator=(DamageTrackManager const &)
{
    return *this;
}

DamageTrackManager* DamageTrackManager::GetInstance()
{
    if (m_Instance == nullptr)
        m_Instance = new DamageTrackManager();

    return m_Instance;
}

void DamageTrackManager::UpdateDamage(EntityDamageData damageData)
{
    intptr_t& target = damageData.target;

    if (m_Trackings.find(target) == m_Trackings.end())
        m_Trackings.insert({ target, new HuntStatistics() });

    HuntStatistics*& statistics = m_Trackings.at(target);
    EntityDamageData& entityData = statistics->entities[damageData.source.index];

    entityData.rawDamage += damageData.rawDamage;
    entityData.elementalDamage += damageData.elementalDamage;

    LOG("[DEBUG] Entity %d -> %08X : %f damage", entityData.source.index, target, entityData.rawDamage + entityData.elementalDamage);
}

HuntStatistics* DamageTrackManager::GetHuntStatisticsBy(intptr_t target)
{
    if (m_Trackings.find(target) == m_Trackings.end())
        return nullptr;

    return m_Trackings.at(target);
}

void DamageTrackManager::DeleteBy(intptr_t target)
{
    if (m_Trackings.find(target) == m_Trackings.end())
        return;

    delete m_Trackings.at(target);
    m_Trackings.erase(target);
}
