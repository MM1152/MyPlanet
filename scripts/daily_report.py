import os
import datetime
from github import Github
from notion_client import Client

# ⚠️ 여기를 본인 정보로 수정하세요
NOTION_DB_ID = "2a31ff65-7f58-80ef-9cd8-cac9ad2a7c19"
TEAM_NAME = "12팀 (정상진, 천민성)"
GITHUB_USERNAME = "MM1152"
TARGET_REPO = "MyPlanet"  # 대상 레포지토리

def create_notion_heading(text, level=1):
    """Notion 헤딩 블록 생성"""
    return {
        "object": "block",
        "type": f"heading_{level}",
        f"heading_{level}": {
            "rich_text": [
                {
                    "type": "text",
                    "text": {"content": text}
                }
            ]
        }
    }

def create_notion_paragraph(text):
    """Notion 문단 블록 생성"""
    return {
        "object": "block",
        "type": "paragraph",
        "paragraph": {
            "rich_text": [
                {
                    "type": "text",
                    "text": {"content": text}
                }
            ]
        }
    }

def create_notion_bullet_list(items):
    """Notion 불릿 리스트 생성"""
    blocks = []
    for item in items:
        if isinstance(item, dict):
            # 이슈 정보가 포함된 경우
            text = f"{item['title']} (#{item['number']})"
            if item.get('url'):
                blocks.append({
                    "object": "block",
                    "type": "bulleted_list_item",
                    "bulleted_list_item": {
                        "rich_text": [
                            {
                                "type": "text",
                                "text": {"content": text},
                                "href": item['url']
                            }
                        ]
                    }
                })
            else:
                blocks.append({
                    "object": "block",
                    "type": "bulleted_list_item",
                    "bulleted_list_item": {
                        "rich_text": [
                            {
                                "type": "text",
                                "text": {"content": text}
                            }
                        ]
                    }
                })
        else:
            # 일반 텍스트인 경우
            blocks.append({
                "object": "block",
                "type": "bulleted_list_item",
                "bulleted_list_item": {
                    "rich_text": [
                        {
                            "type": "text",
                            "text": {"content": str(item)}
                        }
                    ]
                }
            })
    return blocks

def create_divider():
    """Notion 구분선 생성"""
    return {
        "object": "block",
        "type": "divider",
        "divider": {}
    }

def get_yesterday_completed_issues(github, username, repo_name, yesterday_str):
    """전날 완료된 이슈들을 가져오기"""
    print(f"📋 {yesterday_str} 완료된 이슈를 검색 중...")
    
    # 전날에 닫힌 이슈들 검색
    query = f"repo:{username}/{repo_name} is:issue author:{username} closed:{yesterday_str}"
    issues = github.search_issues(query=query)
    
    # 또는 할당받은 이슈 중 전날 닫힌 것들
    assigned_query = f"repo:{username}/{repo_name} is:issue assignee:{username} closed:{yesterday_str}"
    assigned_issues = github.search_issues(query=assigned_query)
    
    # 중복 제거
    all_issues = {}
    for issue in issues:
        all_issues[issue.number] = {
            'title': issue.title,
            'number': issue.number,
            'url': issue.html_url,
            'closed_at': issue.closed_at
        }
    
    for issue in assigned_issues:
        all_issues[issue.number] = {
            'title': issue.title,
            'number': issue.number,
            'url': issue.html_url,
            'closed_at': issue.closed_at
        }
    
    return list(all_issues.values())

def get_today_open_issues(github, username, repo_name):
    """오늘 진행할 열린 이슈들을 가져오기"""
    print("📋 진행 중인 이슈를 검색 중...")
    
    # 내가 작성하거나 할당받은 열린 이슈들
    created_query = f"repo:{username}/{repo_name} is:issue is:open author:{username}"
    assigned_query = f"repo:{username}/{repo_name} is:issue is:open assignee:{username}"
    
    created_issues = github.search_issues(query=created_query)
    assigned_issues = github.search_issues(query=assigned_query)
    
    # 중복 제거
    all_issues = {}
    for issue in created_issues:
        all_issues[issue.number] = {
            'title': issue.title,
            'number': issue.number,
            'url': issue.html_url,
            'created_at': issue.created_at,
            'labels': [label.name for label in issue.labels]
        }
    
    for issue in assigned_issues:
        all_issues[issue.number] = {
            'title': issue.title,
            'number': issue.number,
            'url': issue.html_url,
            'created_at': issue.created_at,
            'labels': [label.name for label in issue.labels]
        }
    
    return list(all_issues.values())

def main():
    # 현재 날짜 (한국 시간 기준)
    kst = datetime.timezone(datetime.timedelta(hours=9))
    now = datetime.datetime.now(kst)
    today = now.strftime("%Y-%m-%d")
    yesterday = (now - datetime.timedelta(days=1)).strftime("%Y-%m-%d")
    
    print(f"📅 {today} 일간보고를 생성합니다...")
    print(f"📅 대상 레포지토리: {GITHUB_USERNAME}/{TARGET_REPO}")
    
    # GitHub API 초기화
    github = Github(os.environ["GITHUB_TOKEN"])
    
    try:
        # 어제 완료된 이슈들
        completed_issues = get_yesterday_completed_issues(github, GITHUB_USERNAME, TARGET_REPO, yesterday)
        print(f"✅ {yesterday} 완료된 이슈: {len(completed_issues)}개")
        
        # 오늘 진행할 이슈들
        open_issues = get_today_open_issues(github, GITHUB_USERNAME, TARGET_REPO)
        print(f"🔄 진행 중인 이슈: {len(open_issues)}개")
        
    except Exception as e:
        print(f"⚠️ 이슈 검색 중 오류: {e}")
        completed_issues = []
        open_issues = []
    
    print("📝 보고서 내용 생성 완료")
    
    # Notion에 저장
    print("📤 Notion에 저장하는 중...")
    
    notion = Client(auth=os.environ["NOTION_TOKEN"])
    
    # Notion 블록 구성
    blocks = []
    
    # 제목
    blocks.append(create_notion_heading(f"{today} 일간보고: {TEAM_NAME}", 1))
    
    # 대상 레포지토리 정보
    blocks.append(create_notion_paragraph(f"📂 대상 레포지토리: {GITHUB_USERNAME}/{TARGET_REPO}"))
    blocks.append(create_divider())
    
    # 전일 보고
    blocks.append(create_notion_heading("전일 보고", 2))
    
    # 완료된 작업
    blocks.append(create_notion_heading("완료", 3))
    if completed_issues:
        blocks.extend(create_notion_bullet_list(completed_issues))
    else:
        blocks.extend(create_notion_bullet_list(["완료된 이슈 없음"]))
    
    # 미완료 작업 (진행 중인 이슈들)
    blocks.append(create_notion_heading("미완료 (사유, 처리)", 3))
    if open_issues:
        incomplete_items = []
        for issue in open_issues[:5]:  # 최대 5개만 표시
            item = issue.copy()
            item['title'] = f"{issue['title']} - 진행중"
            incomplete_items.append(item)
        blocks.extend(create_notion_bullet_list(incomplete_items))
    else:
        blocks.extend(create_notion_bullet_list(["미완료 이슈 없음"]))
    
    blocks.append(create_divider())
    
    # 금일 보고
    blocks.append(create_notion_heading("금일 보고", 2))
    
    # 오늘 할 일 (진행 중인 이슈들 기반)
    today_tasks = []
    if open_issues:
        for issue in open_issues[:3]:  # 우선순위 높은 3개
            today_tasks.append(f"{issue['title']} (#{issue['number']}) 진행")
    
    # 추가 계획된 작업들
    today_tasks.extend([
        "코드 리뷰 및 버그 수정",
        "새로운 기능 개발 계획 수립",
        "문서화 작업"
    ])
    
    blocks.extend(create_notion_bullet_list(today_tasks))
    
    try:
        new_page = notion.pages.create(
            parent={"database_id": NOTION_DB_ID},
            properties={
                "제목": {
                    "title": [
                        {
                            "text": {
                                "content": f"{today} 일간보고"
                            }
                        }
                    ]
                },
                "작성일": {
                    "date": {
                        "start": today
                    }
                }
            },
            children=blocks
        )
        
        print("✅ 일간보고가 성공적으로 Notion에 저장되었습니다!")
        print(f"📄 페이지 ID: {new_page['id']}")
        
    except Exception as e:
        print(f"❌ Notion 저장 중 오류 발생: {e}")
        raise

if __name__ == "__main__":
    main()